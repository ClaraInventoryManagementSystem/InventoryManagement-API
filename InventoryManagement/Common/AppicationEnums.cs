
using System.ComponentModel;

namespace InventoryManagement.Common
{

    #region New InwardReg
    public enum role
    {
        ADMIN = 1,
        ANALYST = 2,
        ASSOCIATE = 3,
    }

    public enum IrOperation
    {
        None = 0,
        Save = 1,
        AssignAnalyst = 2,
        EnterResults = 3,
        ReAssignAnalyst = 4
    }

    public enum ScreenID
    {
        LA_IR_GRID = 1
    }

    public enum DataResultType
    {
        Success = 0,
        Error = 1,
        NoData = 2,
        BadRequest = 3
    }

    public enum AuditLogAction
    {
        LoginSuccess = 1,
        LoginFailed = 2,
        Logout = 3,
        PasswordValidationFailed = 4,
        NewSampleRegistred = 5,
        NewRecordAdded = 6,
        DataUpdatedThroughMR = 7
    }

    #endregion


    public enum employeetype
    {
        Regular =1,
        Contractor = 2,
        Temporary = 3,
        Consultant = 4
    }

    public enum assetcategory
    {        
        [Description("Hardware")]
        Hardware = 1,
        [Description("Software")]
        Software = 2,
        [Description("Furniture")]
        Furniture = 3,
        [Description("Stationery")]
        Stationery = 4,
        [Description("Pantry")]
        Pantry = 5
    }

    public enum assetsubcategory
    {
        [Description("Laptop")]
        Laptop = 1,
        [Description("Desktop")]
        Desktop =2,
        [Description("Monitor")]
        Monitor = 3,
        [Description("Printer")]
        Printer = 4,
        [Description("Switch")]
        Switch = 5,
        [Description("Router")]
        Router = 6
    }

    public enum features
    {
        EMPLOYEE =1,
        ASSET = 2,
        ASSETCATEGORY = 3,
        ATTRIBUTECATEGORY= 4,
        ATTRIBUTES = 5,
        ASSETTRANSACTION = 6,
        COMPANY = 7,
        EMPLOYEEJOBXREF = 8,
        EMPLOYEEROLE = 9,
        EmployeeWork = 10
    }

    public enum jobtype
    {
        UIDeveloper =1,
        DBDeveloper = 2,
        APIDeveloper = 3,
        SoftwareEngineer = 4,
        SrSoftwareEngineer = 5,
        TeamLead = 6,
        ProjectManager = 7,
        NetworkEngineer = 8,
        AccountManager = 9,
        SolutionArchitect = 10
    }

    public enum module
    {
        Assets = 1,
        HR = 2,
        Finance = 3
    }

    public enum privilegemaster
    {
        Edit =1,
        Delete = 2,
        Read = 3,
        FullControl = 4,
        Add = 5
    }

    public enum rolemaster
    {
        Guest = 1,
        EmpGeneric =2,
        Admin =3,
        AssetManager = 4,
        HRManager = 5
    }

    public enum transactiontype
    {
        Purchase =1,
        AssignAllocate = 2,
        Service = 3,
        ServiceReturn = 4,
        Scrap = 5,
        Dispose = 6
    }

    public enum TestWorkflowAction
    {
        Assign =1,
        ReAssign =2,
        Acknowdlge =3,
        SubmitResults =4,
        ModifyResults =5,
        ReleaseResultsQCManager =6,
        ReleaseResultsQAManager =7,
        ReleasePartialCOA =8,
        ReleaseCOA =9
        

    }

    public enum LovGroup
    {
        ArType = 1,
        ContainerType =2,
        QuantityUOM =3,
        StorageMethod =4,
        TestMethod = 5,
        PharmacopoeialTestType = 6,
        OthersTestMethodType = 7,
        Category = 8,
        SubCategory = 9,
        DisposalTimeType =10,
        Condiation = 11,
        TestStatus = 12,
        ARStatus = 13,
        CustomerCategory=14,
        DisposalType = 15,
        SpecificationType = 16,
        Operators = 17,
        MaterialCategory = 18,
        MaterialType = 19,
        MRDataRequestType = 20,
        MRStatus =21,
        AddressType=22,
        UOMSpecification=23
    }

    public enum MRRequestStatus
    {
        Pending =63,
        Approved =64,
        Rejected = 65

    }

    public enum MRType
    {
        Data =1,
        Result =2,
        Price = 3
    }

    public enum MRStatus
    {
        Assigned =1,
        Approved =2,
        Rejected =3
    }

    public enum ArType
    {
        Chemical =1,
        Micro = 2,
        Both = 3
    }
    public enum TestType
    {
        Chemical =1,
        Micro = 2
    }

   public enum ArStatus
    {
        Pending =42,
        COAReleased =43,
        Closed =44,
        Disposed =45

    }
    
    public enum ArCategory
    {
        Pharma =1,
        Others =2
    }

    public enum TestStatus
    {
        PendingAssignment = 29,
        Assigned = 30,
        Acknowledged = 31,
        UnderTesting = 32,
        UnderReviewQCManager = 33,
        UnderReviewQAManager = 34,
        Released =35,
        PartialCOAReleased = 36,
        COAReleased =37,
        Dropped =38,
        Cancelled = 39,
        Disposed = 40,
        Closed = 41

    }
  
    public enum DisposalValueType
    {
        AfterTesting = 24,
        After30Days = 25,
        After45Days = 46
    }

    public enum MaterialDisposalValue
    {
        Yes =48,
       Na = 49
    }
   
}
