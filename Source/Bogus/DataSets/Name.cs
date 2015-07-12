using System;
using Newtonsoft.Json.Linq;

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

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="locale"></param>
        public Name(string locale = "en") : base(locale)
        {
            SupportsGenderFirstNames = Get("male_first_name") != null && Get("female_first_name") != null;
            SupportsGenderLastNames = Get("male_last_name") != null && Get("female_last_name") != null;
        }

        /// <summary>
        /// Get a first name. Getting a gender specific name is only supported on locales that support it. Example, 'ru' supports
        /// male/female names, but not 'en' english.
        /// </summary>
        /// <param name="gender">For locale's that support Gender naming.</param>
        public string FirstName(Gender? gender = null)
        {
            if( SupportsGenderFirstNames )
            {
                gender = gender ?? this.Random.Enum<Gender>();

                if( gender == Gender.Male )
                {
                    return GetRandomArrayItem("male_first_name");
                }
                return GetRandomArrayItem("female_first_name");
            }

            return GetRandomArrayItem("first_name");
        }

        /// <summary>
        /// Get a first name. Getting a gender specific name is only supported on locales that support it. Example, Russian ('ru') supports
        /// male/female names, but Enblish ('en') does not.
        /// </summary>
        /// <param name="gender">For locale's that support Gender naming.</param>
        public string LastName(Gender? gender = null)
        {
            if( SupportsGenderLastNames )
            {
                gender = gender ?? this.Random.Enum<Gender>();

                if( gender == Gender.Male )
                {
                    return GetRandomArrayItem("male_first_name");
                }
                return GetRandomArrayItem("female_first_name");
            }

            return GetRandomArrayItem("last_name");
        }

        /// <summary>
        /// Gets a random prefix for a name
        /// </summary>
        /// <returns></returns>
        public string Prefix()
        {
            return GetRandomArrayItem("prefix");
        }

        /// <summary>
        /// Gets a random suffix for a name
        /// </summary>
        /// <returns></returns>
        public string Suffix()
        {
            return GetRandomArrayItem("suffix");
        }

        /// <summary>
        /// Gets a full name
        /// </summary>
        /// <param name="firstName">Use this first name.</param>
        /// <param name="lastName">use this last name.</param>
        /// <param name="withPrefix">Add a prefix?</param>
        /// <param name="withSuffix">Add a suffix?</param>
        /// <returns></returns>
        public string FindName(string firstName = "", string lastName = "", bool? withPrefix = null, bool? withSuffix = null, Gender? gender = null)
        {
            gender = gender ?? this.Random.Enum<Gender>();
            if( string.IsNullOrWhiteSpace(firstName) )
                firstName = FirstName(gender);
            if( string.IsNullOrWhiteSpace(lastName) )
                lastName = LastName(gender);

            if( !withPrefix.HasValue && !withSuffix.HasValue )
            {
                withPrefix = Random.Bool();
                withSuffix = !withPrefix;
            }
        
            return string.Format("{0} {1} {2} {3}",
                withPrefix.GetValueOrDefault() ? Prefix() : "", firstName, lastName, withSuffix.GetValueOrDefault() ? Suffix() : "")
                .Trim();

        }

        /// <summary>
        /// Gets a random job title.
        /// </summary>
        public string JobTitle()
        {
            var descriptor = JobDescriptor();
            var level = JobArea();
            var job = JobType();

            return string.Format("{0} {1} {2}", descriptor, level, job);
        }

        /// <summary>
        /// Get a job description.
        /// </summary>
        public string JobDescriptor()
        {
            return  GetRandomArrayItem("title.descriptor");
        }

        /// <summary>
        /// Get a job area expertise.
        /// </summary>
        /// <returns></returns>
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
