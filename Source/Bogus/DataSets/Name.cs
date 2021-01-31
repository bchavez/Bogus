namespace Bogus.DataSets
{
   /// <summary>
   /// Methods for generating names
   /// </summary>
   public class Name : DataSet
   {
      public enum Gender
      {
         Male,
         Female
      }

      public readonly bool SupportsGenderFirstNames = false;
      public readonly bool SupportsGenderLastNames = false;
      public readonly bool SupportsGenderPrefixes = false;
      public readonly bool HasFirstNameList = false;

      /// <summary>
      /// Default constructor
      /// </summary>
      public Name(string locale = "en") : base(locale)
      {
         SupportsGenderFirstNames = HasKey("male_first_name", false) && HasKey("female_first_name", false);
         SupportsGenderLastNames = HasKey("male_last_name", false) && HasKey("female_last_name", false);
         SupportsGenderPrefixes = HasKey("male_prefix", false) && HasKey("female_prefix", false);
         HasFirstNameList = HasKey("first_name", false);
      }

      /// <summary>
      /// Switches locale
      /// </summary>
      public Name this[string switchLocale] => new Name(switchLocale);

      /// <summary>
      /// Get a first name. Getting a gender specific name is only supported on locales that support it.
      /// </summary>
      /// <param name="gender">For locale's that support Gender naming.</param>
      public string FirstName(Gender? gender = null)
      {
         if ((gender is null && HasFirstNameList) || !SupportsGenderFirstNames)
            return GetRandomArrayItem("first_name");

         if( gender is null )
            gender = this.Random.Enum<Gender>();

         if( gender == Gender.Male )
         {
            return GetRandomArrayItem("male_first_name");
         }
         return GetRandomArrayItem("female_first_name");
      }

      /// <summary>
      /// Get a last name. Getting a gender specific name is only supported on locales that support it.
      /// </summary>
      /// <param name="gender">For locale's that support Gender naming.</param>
      public string LastName(Gender? gender = null)
      {
         if( SupportsGenderLastNames )
         {
            gender ??= this.Random.Enum<Gender>();

            if( gender == Gender.Male )
            {
               return GetRandomArrayItem("male_last_name");
            }
            return GetRandomArrayItem("female_last_name");
         }

         return GetRandomArrayItem("last_name");
      }

      /// <summary>
      /// Get a full name, concatenation of calling FirstName and LastName.
      /// </summary>
      /// <param name="gender">Gender of the name if supported by the locale.</param>
      public string FullName(Gender? gender = null)
      {
         // PR#148 - 'ru' locale requires a gender to be
         // specified for both first and last name. Gender is not
         // picked when 'en' locale is specified because
         // SupportsGenderLastNames = false when 'en' is used.
         // SupportsGenderLastNames is false because 'en' doesn't have
         // en: male_last_name and en: female_last_name JSON fields.
         if ( SupportsGenderFirstNames && SupportsGenderLastNames )
           gender ??= this.Random.Enum<Gender>();

         return $"{FirstName(gender)} {LastName(gender)}";
      }

      /// <summary>
      /// Gets a random prefix for a name.
      /// </summary>
      public string Prefix(Gender? gender = null)
      {
         gender ??= this.Random.Enum<Gender>();
         if( SupportsGenderPrefixes )
         {
            if( gender == Gender.Male )
            {
               return GetRandomArrayItem("male_prefix");
            }
            return GetRandomArrayItem("female_prefix");
         }
         return GetRandomArrayItem("prefix");
      }

      /// <summary>
      /// Gets a random suffix for a name.
      /// </summary>
      public string Suffix()
      {
         return GetRandomArrayItem("suffix");
      }

      /// <summary>
      /// Gets a full name.
      /// </summary>
      /// <param name="firstName">Use this first name.</param>
      /// <param name="lastName">use this last name.</param>
      /// <param name="withPrefix">Add a prefix?</param>
      /// <param name="withSuffix">Add a suffix?</param>
      public string FindName(string firstName = "", string lastName = "", bool? withPrefix = null, bool? withSuffix = null, Gender? gender = null)
      {
         gender ??= this.Random.Enum<Gender>();
         if( string.IsNullOrWhiteSpace(firstName) )
            firstName = FirstName(gender);
         if( string.IsNullOrWhiteSpace(lastName) )
            lastName = LastName(gender);

         if( !withPrefix.HasValue && !withSuffix.HasValue )
         {
            withPrefix = Random.Bool();
            withSuffix = !withPrefix;
         }

         var prefix = withPrefix.GetValueOrDefault() ? Prefix(gender) : "";
         var suffix = withSuffix.GetValueOrDefault() ? Suffix() : "";

         return $"{prefix} {firstName} {lastName} {suffix}".Trim();
      }

      /// <summary>
      /// Gets a random job title.
      /// </summary>
      public string JobTitle()
      {
         var descriptor = JobDescriptor();
         var level = JobArea();
         var job = JobType();

         return $"{descriptor} {level} {job}";
      }

      /// <summary>
      /// Get a job description.
      /// </summary>
      public string JobDescriptor()
      {
         return GetRandomArrayItem("title.descriptor");
      }

      /// <summary>
      /// Get a job area expertise.
      /// </summary>
      public string JobArea()
      {
         return GetRandomArrayItem("title.level");
      }

      /// <summary>
      /// Get a type of job.
      /// </summary>
      public string JobType()
      {
         return GetRandomArrayItem("title.job");
      }
   }
}
