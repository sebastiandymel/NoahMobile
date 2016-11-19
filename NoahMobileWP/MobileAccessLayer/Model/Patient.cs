using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himsa.Noah.MobileAccessLayer
{
    public enum Gender{Unknown = 0, Male = 1, Female = 2}

    /// <summary>
    /// Holds the Patient demographics object
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// Gets or sets the Id.
        /// The Id is a unique identification of the object/record in the database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the patient GUID.
        /// </summary>
        public string PatientGUID { get; set; }

        /// <summary>
        /// Gets or sets the patient no.
        /// Max length is 20 characters.
        /// </summary>
        public string PatientNo { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// Max length is 3 characters (User Initials).
        /// <para>If the CreatedBy user does not exists CreatedBy must be set to "???".</para>
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        public string LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the active patient. Can be 1 (active) or 2 (inactive). If set to a value not 1 or 2, it will be forced to a value of 1.
        /// </summary>
        public Int16 ActivePatient { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// Max length is 30 characters.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// Max length is 30 characters.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// Max length is 30 characters.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the salutation.
        /// Max length is 30 characters.
        /// </summary>
        public string Salutation { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// Max length is 30 characters.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the address1.
        /// Max length is 30 characters.
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the address2.
        /// Max length is 30 characters.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the address3.
        /// Max length is 30 characters.
        /// </summary>
        public string Address3 { get; set; }

        /// <summary>
        /// Gets or sets the province.
        /// Max length is 30 characters.
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// Gets or sets the zip.
        /// Max length is 20 characters.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// Max length is 30 characters.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// Max length is 30 characters.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the home phone.
        /// Max length is 30 characters.
        /// </summary>
        public string HomeTelephone { get; set; }

        /// <summary>
        /// Gets or sets the work phone.
        /// Max length is 30 characters.
        /// </summary>
        public string WorkTelephone { get; set; }

        /// <summary>
        /// Gets or sets the mobile phone.
        /// Max length is 30 characters.
        /// </summary>
        public string MobileTelephone { get; set; }

        /// <summary>
        /// Gets or sets the E mail.
        /// Max length is 50 characters.
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// Gets or sets the SS number.
        /// Max length is 20 characters.
        /// </summary>
        public string SSNumber { get; set; }

        /// <summary>
        /// Gets or sets the physician.
        /// Max length is 30 characters.
        /// </summary>
        public string Physician { get; set; }

        /// <summary>
        /// Gets or sets the referral.
        /// Max length is 30 characters.
        /// </summary>
        public string Referral { get; set; }

        /// <summary>
        /// Gets or sets the insurance1.
        /// Max length is 30 characters.
        /// </summary>
        public string Insurance1 { get; set; }

        /// <summary>
        /// Gets or sets the insurance2.
        /// Max length is 30 characters.
        /// </summary>
        public string Insurance2 { get; set; }

        /// <summary>
        /// Gets or sets the Other1.
        /// Max length is 30 characters.
        /// </summary>
        public string Other1 { get; set; }

        /// <summary>
        /// Gets or sets the Other2.
        /// Max length is 30 characters.
        /// </summary>
        public string Other2 { get; set; }

       
    }
}
