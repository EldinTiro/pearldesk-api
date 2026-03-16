namespace PearlDesk.Domain.Identity;

public static class Roles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string ClinicOwner = "ClinicOwner";
    public const string ClinicAdmin = "ClinicAdmin";
    public const string Dentist = "Dentist";
    public const string Hygienist = "Hygienist";
    public const string Receptionist = "Receptionist";
    public const string BillingStaff = "BillingStaff";
    public const string ReadOnly = "ReadOnly";

    public static readonly IReadOnlyList<string> All =
    [
        SuperAdmin, ClinicOwner, ClinicAdmin, Dentist, Hygienist, Receptionist, BillingStaff, ReadOnly
    ];

    public static readonly IReadOnlyList<string> TenantRoles =
    [
        ClinicOwner, ClinicAdmin, Dentist, Hygienist, Receptionist, BillingStaff, ReadOnly
    ];
}


