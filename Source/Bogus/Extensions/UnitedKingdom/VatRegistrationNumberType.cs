namespace Bogus.Extensions.UnitedKingdom;

/// <summary>
/// Enum for the different types of UK VAT registration numbers.
/// </summary>
public enum VatRegistrationNumberType
{
   /// <summary>
   /// Standard company, for example GB### #### ##
   /// </summary>
   Standard = 0,

   /// <summary>
   /// Branch trader, similar to <see cref="Standard"/> with a 3 digit suffix. For example, GB### #### ## ###
   /// </summary>
   BranchTrader = 1,

   /// <summary>
   /// Government department. for example GBGD###
   /// </summary>
   GovernmentDepartment = 2,

   /// <summary>
   /// Health authority, for example GBHA###
   /// </summary>
   HealthAuthority = 3
}