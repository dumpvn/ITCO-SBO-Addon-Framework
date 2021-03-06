﻿namespace ITCO.SboAddon.Framework.Extensions
{
    using System.Linq;
    using Helpers;
    using SAPbobsCOM;
    using ITCO.SboAddon.Framework.Queries;

    /// <summary>
    /// Business Partner Extensions
    /// </summary>
    public static class BusinessPartnerExtensions
    {
        /// <summary>
        /// SetCurrentLine by CntctCode
        /// </summary>
        /// <param name="businessPartners"></param>
        /// <param name="contactCode"></param>
        /// <returns></returns>
        public static bool SetContactEmployeesLineByContactCode(this BusinessPartners businessPartners, int contactCode)
        {
            var cardCode = businessPartners.CardCode;
            var contactEmployees = businessPartners.ContactEmployees;
            using (var query = new SboRecordsetQuery(FrameworkQueries.Instance.SetContactEmployeesLineByContactCodeQuery(cardCode, contactCode)))
            {
                if (query.Count == 0) return false;

                var lineNum = (int)query.Result.First().Item(0).Value;
                SboApp.Logger.Debug($"CntctCode {contactCode} is LineNum {lineNum} for CardCode {cardCode}");

                contactEmployees.SetCurrentLine(lineNum);
                return true;
            }
        }

        /// <summary>
        /// SetCurrentLine by "Contact ID"
        /// </summary>
        /// <param name="businessPartners"></param>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public static bool SetContactEmployeesLineByContactId(this BusinessPartners businessPartners, string contactId)
        {
            var cardCode = businessPartners.CardCode;
            var contactEmployees = businessPartners.ContactEmployees;

            using (var query = new SboRecordsetQuery(FrameworkQueries.Instance.SetContactEmployeesLineByContactIdQuery(cardCode, contactId)))
            {
                if (query.Count == 0) return false;

                var lineNum = (int)query.Result.First().Item(0).Value;
                SboApp.Logger.Debug($"Contact ID '{contactId}' is LineNum {lineNum} for CardCode {cardCode}");

                contactEmployees.SetCurrentLine(lineNum);
                return true;
            }
        }

        /// <summary>
        /// Set Next Card Code
        /// </summary>
        /// <param name="card">BusinessPartners object</param>
        /// <param name="serieId">NNM1 Serie Id</param>
        public static void SetNextCardCode(this BusinessPartners card, int? serieId = null)
        {
            var response = DocumentSeriesHelper.GetNextNumber(BoObjectTypes.oBusinessPartners, "C", serieId);
            card.CardCode = response.NextNumber;
            card.Series = response.Series;
        }
    }
}
