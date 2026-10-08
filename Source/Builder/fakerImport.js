// Imports faker.js (TypeScript layout, v9+) locale definitions and converts
// them into the legacy JSON shape that Bogus DataSets consume
// (ie: `address.city_prefix`, `name.female_first_name`, `#{Name.last_name}` tokens).
//
// Locale TypeScript sources are bundled on the fly with esbuild, so there is
// no need to install or build the faker.js workspace.

const esbuild = require("esbuild");
const path = require("path");
const fs = require("fs");
const l = require("lodash");

// FAKERJS_LOCALES can point at a faker.js checkout other than the submodule (ie: for experiments).
const localesRoot = path.resolve(process.env.FAKERJS_LOCALES || path.join(__dirname, "../fakerjs/src/locales"));

// faker.js locale folder -> Bogus locale code. Keeps the codes that
// shipped with earlier versions of Bogus (and ExtensionsForCultureInfo) working.
const localeCodeMap = {
   cs_CZ: "cz",
   ka_GE: "ge",
   en_IN: "en_IND",
};

// Locales that are skipped. `base` is shared data merged into `en`.
const skipLocales = ["base"];

const log = msg => console.log(msg);

function listLocaleFolders() {
   return fs.readdirSync(localesRoot, { withFileTypes: true })
      .filter(d => d.isDirectory() && !skipLocales.includes(d.name))
      .map(d => d.name)
      .sort();
}

function loadLocale(folder) {
   const result = esbuild.buildSync({
      entryPoints: [path.join(localesRoot, folder, "index.ts")],
      bundle: true,
      write: false,
      format: "cjs",
      platform: "node",
      logLevel: "silent",
   });
   const m = { exports: {} };
   new Function("module", "exports", "require", result.outputFiles[0].text)(m, m.exports, require);
   return m.exports.default;
}

// faker.js `{{module.method}}` handlebars -> [bogus category, bogus key].
// Anything not listed here can't be expressed as a Bogus `#{}` token and
// causes the containing pattern to be dropped (with a warning).
const tokenMap = {
   "person.firstName": ["name", "first_name"],
   "person.lastName": ["name", "last_name"],
   "person.last_name.generic": ["name", "last_name"],
   "person.first_name.generic": ["name", "first_name"],
   "person.first_name.male": ["name", "male_first_name"],
   "person.first_name.female": ["name", "female_first_name"],
   "person.last_name.male": ["name", "male_last_name"],
   "person.last_name.female": ["name", "female_last_name"],
   "person.prefix": ["name", "prefix"],
   "person.suffix": ["name", "suffix"],
   "person.fullName": ["name", "name"],
   "person.name": ["name", "name"],

   "location.city_prefix": ["address", "city_prefix"],
   "location.city_suffix": ["address", "city_suffix"],
   "location.common_street_suffix": ["address", "common_street_suffix"],
   "location.street_prefix": ["address", "street_prefix"],
   "location.street_suffix": ["address", "street_suffix"],
   "location.street_name_part": ["address", "street_root"],
   "location.street_name": ["address", "street_names"],
   "location.city_name": ["address", "city_name"],
   "location.buildingNumber": ["address", "building_number"],
   "location.street": ["address", "street_name"],
   "location.secondaryAddress": ["address", "secondary_address"],
   "location.city": ["address", "city"],
   "location.state": ["address", "state"],
   "location.zipCode": ["address", "postcode"],
   "location.streetAddress": ["address", "street_address"],
   "location.county": ["address", "county"],
   "location.country": ["address", "country"],

   "company.legal_entity_type": ["company", "suffix"],
   "company.name": ["company", "name"],
   "company.company_name": ["company", "company_names"],
   "company.adjective": ["company", "adjective"],
   "company.noun": ["company", "noun"],
   "commerce.department": ["commerce", "department"],

   "cell_phone.common_cell_prefix": ["cell_phone", "common_cell_prefix"],

   "team.creature": ["team", "creature"],
   "team.suffix": ["team", "suffix"],

   "hacker.verb": ["hacker", "verb"],
   "hacker.adjective": ["hacker", "adjective"],
   "hacker.noun": ["hacker", "noun"],
   "hacker.abbreviation": ["hacker", "abbreviation"],
   "hacker.ingverb": ["hacker", "ingverb"],
};

class PatternConverter {
   constructor(warn) {
      this.warn = warn;
   }

   // Converts one faker.js pattern into a Bogus pattern, returns null if it can't be converted.
   convert(pattern, ownCategory) {
      let ok = true;
      const converted = pattern.replace(/\{\{\s*([^}]+?)\s*\}\}/g, (all, token) => {
         const mapped = tokenMap[token];
         if (!mapped) {
            ok = false;
            this.warn(`Unsupported token {{${token}}} in pattern "${pattern}"`);
            return all;
         }
         const [category, key] = mapped;
         return category === ownCategory ? `#{${key}}` : `#{${category}.${key}}`;
      });
      return ok ? converted : null;
   }

   convertAll(patterns, ownCategory) {
      return patterns
         .map(p => this.convert(p, ownCategory))
         .filter(p => p !== null);
   }
}

// faker.js weights patterns with `{value, weight}`; Bogus picks uniformly, so repeat by weight.
function expandWeighted(arr) {
   return l.flatMap(arr, item => {
      if (typeof item === "string") return [item];
      return l.times(Math.max(1, item.weight || 1), () => item.value);
   });
}

// faker.js uses `!` for a non-zero digit, Bogus only knows `#`.
const fixPhone = s => s.replace(/!/g, "#");

const asArray = v => (v === undefined || v === null ? undefined : (Array.isArray(v) ? v : [v]));
const stringify = arr => arr && arr.map(x => (typeof x === "number" ? x.toString() : x));
const nonEmpty = v => (Array.isArray(v) ? v.length > 0 : v !== undefined && v !== null);

// Sets obj[key] = value only if the value is present and non-empty.
function put(obj, key, value) {
   if (nonEmpty(value)) obj[key] = value;
}

function section(root, name) {
   if (!root[name]) root[name] = {};
   return root[name];
}

// faker.js dropped the full mime-db listing from its locale data, Bogus' `System.MimeType()` and
// `System.FileExt()` still want it, so it is sourced from mime-db directly.
function mimeTypes() {
   const db = require("mime-db");
   return Object.keys(db).map(mime => ({
      mime,
      source: db[mime].source,
      compressible: db[mime].compressible,
      extensions: db[mime].extensions,
   }));
}

function convertLocale(folder, def, base) {
   const warnings = [];
   const warn = msg => warnings.push(msg);
   const pc = new PatternConverter(warn);

   const out = {};
   out.title = def.metadata && def.metadata.title ? def.metadata.title : folder;

   // ---- address (location)
   const loc = def.location || {};
   const address = {};
   put(address, "city_prefix", loc.city_prefix);
   put(address, "city_suffix", loc.city_suffix);
   put(address, "county", loc.county);
   put(address, "country", loc.country);
   put(address, "building_number", loc.building_number);
   put(address, "street_suffix", loc.street_suffix);
   put(address, "common_street_suffix", loc.common_street_suffix);
   put(address, "street_prefix", loc.street_prefix);
   put(address, "street_root", loc.street_name_part);
   put(address, "secondary_address", loc.secondary_address);
   put(address, "postcode", asArray(loc.postcode));
   put(address, "state", loc.state);
   put(address, "state_abbr", loc.state_abbr);
   put(address, "city_name", loc.city_name);
   put(address, "street_names", loc.street_name);
   if (loc.city_pattern) put(address, "city", pc.convertAll(loc.city_pattern, "address"));
   if (loc.street_pattern) put(address, "street_name", pc.convertAll(loc.street_pattern, "address"));
   if (loc.street_address && loc.street_address.normal) {
      put(address, "street_address", pc.convertAll([loc.street_address.normal], "address"));
   }
   if (loc.direction) {
      put(address, "direction", [].concat(loc.direction.cardinal || [], loc.direction.ordinal || []));
      put(address, "direction_abbr", [].concat(loc.direction.cardinal_abbr || [], loc.direction.ordinal_abbr || []));
   }
   if (base) {
      const codes = base.location && base.location.country_code;
      if (codes) {
         address.country_code = codes.map(c => c.alpha2);
         address.country_code_alpha_3 = codes.map(c => c.alpha3);
      }
      put(address, "time_zone", base.location && base.location.time_zone);
   }
   if (!l.isEmpty(address)) out.address = address;

   // ---- company
   const comp = def.company || {};
   const company = {};
   put(company, "suffix", comp.legal_entity_type);
   put(company, "company_names", comp.company_name);
   put(company, "adjective", comp.adjective);
   put(company, "descriptor", comp.descriptor);
   put(company, "noun", comp.noun);
   put(company, "bs_verb", comp.buzz_verb);
   put(company, "bs_adjective", comp.buzz_adjective);
   put(company, "bs_noun", comp.buzz_noun);
   if (comp.name_pattern) put(company, "name", pc.convertAll(comp.name_pattern, "company"));
   if (!l.isEmpty(company)) out.company = company;

   // ---- internet
   const net = def.internet || {};
   const internet = {};
   put(internet, "free_email", net.free_email);
   put(internet, "example_email", net.example_email);
   put(internet, "domain_suffix", net.domain_suffix);
   if (!l.isEmpty(internet)) out.internet = internet;

   // ---- database
   const db = l.merge({}, base && base.database, def.database);
   const database = {};
   put(database, "collation", db.collation);
   put(database, "column", db.column);
   put(database, "engine", db.engine);
   put(database, "type", db.type);
   if (!l.isEmpty(database)) out.database = database;

   // ---- lorem
   if (def.lorem && def.lorem.word) out.lorem = { words: def.lorem.word };

   // ---- name (person)
   const per = def.person || {};
   const name = {};
   const fn = per.first_name || {};
   const male = fn.male || [];
   const female = fn.female || [];
   put(name, "male_first_name", male);
   put(name, "female_first_name", female);
   if (fn.generic) put(name, "first_name", [].concat(fn.generic, female, male));
   const ln = per.last_name || {};
   put(name, "last_name", ln.generic);
   put(name, "male_last_name", ln.male);
   put(name, "female_last_name", ln.female);
   if (!ln.generic && (ln.male || ln.female)) {
      put(name, "last_name", [].concat(ln.female || [], ln.male || []));
   }
   const pre = per.prefix || {};
   put(name, "prefix", [].concat(pre.generic || [], pre.female || [], pre.male || []));
   // Gendered prefixes switch Bogus' `Name.Prefix(gender)` to gender-only pools, which would
   // exclude generic prefixes like `Dr.`. Only emit them if the locale has no generic prefixes.
   if (!pre.generic) {
      put(name, "female_prefix", pre.female);
      put(name, "male_prefix", pre.male);
   }
   put(name, "suffix", per.suffix);
   put(name, "gender", per.gender);
   if (per.job_descriptor || per.job_area || per.job_type) {
      name.title = {};
      put(name.title, "descriptor", per.job_descriptor);
      put(name.title, "level", per.job_area);
      put(name.title, "job", per.job_type);
   }
   if (per.name) put(name, "name", pc.convertAll(expandWeighted(per.name), "name"));
   if (!l.isEmpty(name)) out.name = name;

   // ---- phone_number / cell_phone
   const ph = def.phone_number || {};
   if (ph.format && ph.format.human) {
      out.phone_number = { formats: ph.format.human.map(fixPhone) };
   }
   if (def.cell_phone) {
      const cell = {};
      put(cell, "common_cell_prefix", stringify(def.cell_phone.common_cell_prefix));
      if (def.cell_phone.formats) {
         put(cell, "formats", pc.convertAll(def.cell_phone.formats.map(fixPhone), "cell_phone"));
      }
      if (!l.isEmpty(cell)) out.cell_phone = cell;
   }

   // ---- commerce
   const com = def.commerce || {};
   const commerce = {};
   put(commerce, "color", def.color && def.color.human);
   put(commerce, "department", com.department);
   put(commerce, "product_description", com.product_description);
   if (com.product_name) {
      const pn = {};
      put(pn, "adjective", com.product_name.adjective);
      put(pn, "material", com.product_name.material);
      put(pn, "product", com.product_name.product);
      if (!l.isEmpty(pn)) commerce.product_name = pn;
   }
   if (!l.isEmpty(commerce)) out.commerce = commerce;

   // ---- team
   const tm = def.team || {};
   const team = {};
   put(team, "creature", tm.creature);
   put(team, "suffix", tm.suffix);
   if (tm.name) put(team, "name", pc.convertAll(tm.name, "team"));
   if (!l.isEmpty(team)) out.team = team;

   // ---- hacker
   const hk = l.merge({}, base && base.hacker, def.hacker);
   const hacker = {};
   put(hacker, "abbreviation", hk.abbreviation);
   put(hacker, "adjective", hk.adjective);
   put(hacker, "noun", hk.noun);
   put(hacker, "verb", hk.verb);
   put(hacker, "ingverb", hk.ingverb);
   if (hk.phrase) put(hacker, "phrase", pc.convertAll(hk.phrase, "hacker"));
   if (!l.isEmpty(hacker)) out.hacker = hacker;

   // ---- app
   if (def.app) {
      const app = {};
      put(app, "name", def.app.name);
      put(app, "version", def.app.version);
      if (def.app.author) put(app, "author", pc.convertAll(def.app.author, "app").map(a => a));
      if (!l.isEmpty(app)) out.app = app;
   }

   // ---- finance
   if (def.finance) {
      const fin = {};
      put(fin, "account_type", def.finance.account_type);
      put(fin, "transaction_type", def.finance.transaction_type);
      if (def.finance.currency) {
         fin.currency = def.finance.currency.map(c => ({ name: c.name, code: c.code, symbol: c.symbol }));
      }
      if (!l.isEmpty(fin)) out.finance = fin;
   }

   // ---- date
   if (def.date) {
      const date = {};
      put(date, "month", def.date.month);
      put(date, "weekday", def.date.weekday);
      if (!l.isEmpty(date)) out.date = date;
   }

   // ---- system
   if (base && base.system) {
      const system = {};
      put(system, "directoryPaths", base.system.directory_path);
      put(system, "mimeTypes", mimeTypes());
      if (!l.isEmpty(system)) out.system = system;
   }

   // ---- vehicle
   if (def.vehicle) {
      const v = {};
      put(v, "manufacturer", def.vehicle.manufacturer);
      put(v, "model", def.vehicle.model);
      put(v, "type", def.vehicle.type);
      put(v, "fuel", def.vehicle.fuel);
      if (!l.isEmpty(v)) out.vehicle = v;
   }

   // ---- music
   if (def.music) {
      const m = {};
      put(m, "genre", def.music.genre);
      if (!l.isEmpty(m)) out.music = m;
   }

   pruneNulls(out);
   ensureAllArraysAreStrings(out);
   verifyTokens(out, warn);

   return { locale: out, warnings };
}

// Some locales explicitly set `null` (ie: `date.month.abbr`), meaning "no data".
function pruneNulls(obj) {
   l.forOwn(obj, (v, k) => {
      if (v === null || v === undefined) {
         delete obj[k];
      } else if (l.isPlainObject(v)) {
         pruneNulls(v);
         if (l.isEmpty(v)) delete obj[k];
      }
   });
}

function ensureAllArraysAreStrings(obj) {
   l.forOwn(obj, (v, k) => {
      if (Array.isArray(v)) {
         obj[k] = v.map(x => (typeof x === "number" ? x.toString() : x));
      } else if (l.isPlainObject(v)) {
         ensureAllArraysAreStrings(v);
      }
   });
}

// Every `#{key}` / `#{category.key}` must resolve in this locale or in the english fallback.
// `en` is verified by the caller (it is the fallback), others may reference en.
function verifyTokens(out, warn) {
   l.forOwn(out, (sec, category) => {
      if (!l.isPlainObject(sec)) return;
      l.forOwn(sec, (arr, key) => {
         if (!Array.isArray(arr)) return;
         const bad = arr.filter(s => typeof s === "string" && /#\{[^}]*\}/.test(s));
         if (bad.length) out.__tokens = (out.__tokens || []).concat(bad.map(b => ({ category, key, value: b })));
      });
   });
}

function resolveTokenReferences(locales) {
   const en = locales.en && locales.en.locale;
   const has = (loc, cat, key) => loc[cat] && loc[cat][key] !== undefined;
   l.forOwn(locales, (entry, code) => {
      const loc = entry.locale;
      const refs = loc.__tokens || [];
      delete loc.__tokens;
      refs.forEach(ref => {
         const arr = loc[ref.category][ref.key];
         const kept = arr.filter(s => {
            const tokens = [...s.matchAll(/#\{([^}]*)\}/g)].map(m => m[1].toLowerCase());
            return tokens.every(t => {
               const parts = t.split(".");
               const cat = parts.length === 1 ? ref.category : parts[0];
               const key = parts.length === 1 ? parts[0] : parts[1];
               return has(loc, cat, key) || (en && has(en, cat, key));
            });
         });
         if (kept.length !== arr.length) {
            entry.warnings.push(`Dropped ${arr.length - kept.length} unresolved pattern(s) from ${ref.category}.${ref.key}`);
            if (kept.length === 0) {
               delete loc[ref.category][ref.key];
            } else {
               loc[ref.category][ref.key] = kept;
            }
         }
      });
   });
}

// Imports all faker.js locales. Returns { [bogusLocaleCode]: { locale, warnings } }
function importAll(onlyFolders) {
   const baseDef = loadLocale("base");
   const result = {};
   const folders = onlyFolders || listLocaleFolders();
   folders.forEach(folder => {
      const code = localeCodeMap[folder] || folder;
      log(`Importing ${folder} -> ${code}`);
      const def = loadLocale(folder);
      result[code] = convertLocale(folder, def, folder === "en" ? baseDef : null);
   });
   resolveTokenReferences(result);
   return result;
}

// Merges the hand-maintained `data_extend` overrides (arrays are replaced, not merged by index).
function mergeExtend(locale, code, extendDir) {
   const extendPath = path.resolve(extendDir, `${code}.locale.json`);
   if (!fs.existsSync(extendPath)) return locale;
   const extendData = JSON.parse(fs.readFileSync(extendPath, "utf8"));
   const replacer = (objValue, srcValue) => {
      if (Array.isArray(objValue)) return srcValue;
   };
   return l.mergeWith(locale, extendData, replacer);
}

// Writes `<code>.locale.json` files (CRLF) into outDir and returns the written file names.
function writeLocales(imported, outDir, extendDir) {
   fs.mkdirSync(outDir, { recursive: true });
   return Object.keys(imported).sort().map(code => {
      const merged = mergeExtend(imported[code].locale, code, extendDir);
      const fileName = `${code}.locale.json`;
      const json = JSON.stringify(merged, null, 2).replace(/\r?\n/g, "\r\n");
      fs.writeFileSync(path.join(outDir, fileName), json);
      return fileName;
   });
}

module.exports = { importAll, writeLocales, mergeExtend, listLocaleFolders, localeCodeMap };

if (require.main === module) {
   // node fakerImport.js <outDir> [folder...]
   const outDir = path.resolve(process.argv[2] || "../Bogus/data");
   const only = process.argv.length > 3 ? process.argv.slice(3) : undefined;
   const imported = importAll(only);
   writeLocales(imported, outDir, path.resolve(__dirname, "../Bogus/data_extend"));
   Object.keys(imported).forEach(code => imported[code].warnings.forEach(w => log(`WARN [${code}] ${w}`)));
}
