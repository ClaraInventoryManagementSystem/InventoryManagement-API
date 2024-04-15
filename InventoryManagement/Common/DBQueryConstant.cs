using Microsoft.AspNetCore.Routing.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace InventoryManagement.Common
{
    internal static class DBQueryConstant
    {

        #region AuditLog_queries

        internal static string ADD_SYSTEM_AUDIT_LOG = @"INSERT INTO system_audit
                                                        (
                                                        ACTION,
                                                        AUDIT_DATE,
                                                        USER_ID,
                                                        ROLE,
                                                        DESCRIPTION,
                                                        REFERENCE_ID,
                                                        REMARKS)
                                                        VALUES
                                                        (
                                                        @Action,
                                                        NOW(),
                                                        @userId,
                                                        (select ROLE_CODE from app_role where ROLE_ID=@RoleId) ,
                                                        @Deescription,
                                                        @REFERENCE_ID,
                                                        @REMARKS);

                                                        select * from system_audit;
                                                        ";
        internal static string GET_SYSTEM_AUDIT_COUNT = @" select count(*) as count from system_audit WHERE AUDIT_DATE  BETWEEN '{0}' AND '{1}'";
        internal static string GET_SYSTEM_AUDIT_LOG = @"select sa.*, concat(U.USER_FIRST_NAME,' ', U.USER_LAST_NAME) AS USER_NAME from system_audit sa
inner join app_user u on sa.USER_ID=u.USER_ID
WHERE sa.AUDIT_DATE  BETWEEN @lowerdate AND @upperdate LIMIT @perPage OFFSET @pageStart;";

        internal static string AUDIT_AR_DETAILS = @"select  IR.AR_NUMBER,C.CUSTOMER_SHORT_NAME,L.LOCATION_SHORT_NAME,
M.MATERIAl_SHORT_NAME,IR.REGISTER_DATE from inward_register IR 
inner join ir_material IRM on IR.INWARD_REGISTER_ID = IRM.INWARD_REGISTER_ID
inner join customer C ON IR.CUSTOMER_ID = C.CUSTOMER_ID
inner join customer_location L on IR.CUSTOMER_LOCATION_ID = L.CUSTOMER_LOC_ID
inner join material M on IRM.MATERIAL_ID = M.MATERIAL_ID
where IR.INWARD_REGISTER_ID=@inward_id;";
        internal static string GET_AUDIT_LOG_BY_SAMPLE = @"select IMT.IR_TEST_ID,
T.TEST_SHORT_NAME AS TEST_NAME,
 IM.CUSTOMER_LOT_NO AS LOT_NUMBER,
  ITW.IR_STATUS_DATE AS EVENT_DATE,
   CASE 
   WHEN (ITW.IR_TEST_STATUS = 29) THEN 'Registred'
   WHEN (ITW.IR_TEST_STATUS = 30) THEN 'Assigned'
   WHEN (ITW.IR_TEST_STATUS = 32) THEN 'Acknowldged'
   WHEN (ITW.IR_TEST_STATUS = 33) THEN  'Results Submitted'
   WHEN (ITW.IR_TEST_STATUS = 34) THEN 'QC Manager Approved Results'
   WHEN (ITW.IR_TEST_STATUS = 35) THEN 'QA Manger Approved Results'
   WHEN (ITW.IR_TEST_STATUS = 36)  THEN 'COA Released'
   END
   AS EVENT_NAME,
   ( select CONCAT(USER_FIRST_NAME,' ', USER_LAST_NAME) from app_user where USER_ID= ITW.IR_STATUS_USER) AS USER,
   ( select ROLE_DESCRIPTION from app_role where ROLE_ID = ITW.IR_STATUS_USER_ROLE) AS USER_ROLE,
   ITW.REMARKS as EVENT_REMARKS
    from ir_material_test IMT
INNER JOIN ir_material IM ON IMT.IR_MATERIAL_ID = IM.IR_MATERIAL_ID and IM.INWARD_REGISTER_ID = @inward_id
INNER JOIN ir_test_workflow ITW ON IMT.IR_TEST_ID = ITW.IR_TEST_ID
INNER JOIN inward_register IR ON IM.INWARD_REGISTER_ID = IR.INWARD_REGISTER_ID
INNER JOIN test T on IMT.TEST_ID = T.TEST_ID
LEFT JOIN ir_workflow IW ON IW.INWARD_REGISTER_ID = IR.INWARD_REGISTER_ID
WHERE IR.INWARD_REGISTER_ID  =@inward_id order by ITW.IR_STATUS_DATE asc;
";
        #endregion      

        #region COA
        internal static string COA_DATA_CUSTOMER_INFO = @"select  cl.LOCATION_SHORT_NAME,
cl.STREET_ADDRESS1,cl.STREET_ADDRESS2,cl.CITY,cl.STATE,cl.ZIP_CODE,
m.MATERIAL_LONG_NAME,
lc.CONTACT_EMAIL,
iw.AR_NUMBER,iw.REGISTER_DATE,
(select CONCAT(USER_FIRST_NAME,'',USER_LAST_NAME) from app_user where USER_ID = @current_user_id) AS USER_NAME
 from
 inward_register iw 
inner join ir_material irm on irm.INWARD_REGISTER_ID = iw.INWARD_REGISTER_ID
inner join customer_location cl on iw.CUSTOMER_LOCATION_ID = cl.CUSTOMER_LOC_ID
inner join material m on m.MATERIAL_ID = irm.MATERIAL_ID
inner join customer_location_contact lc on iw.CUSTOMER_CONTACTID = lc.CUSTOMER_LOC_CNCT_ID
WHERE iw.AR_NUMBER in (@ar_number);
";

        internal static string COA_TEST_DATA = @"select  
t.TEST_LONG_NAME,imt.REMARKS,
CONCAT (imt.TEST_RESULT,(select LONG_NAME from lov_entry where LOV_ENTRY_ID=imt.TEST_RESULT_UOM )) AS RESULTS,
s.SPECIFICATION_LONG_NAME
 from
ir_material_test imt
inner join test t on imt.TEST_ID = t.TEST_ID
inner join specification s on t.SPECIFICATION_ID = s.SPECIFICATION_ID
WHERE imt.IR_TEST_ID in(@test_id_filter)";

        internal static string GET_EMAIL_LIST = @"select cli.CONTACT_EMAIL,c.CUSTOMER_ID from
inward_register iw 
inner join customer c on iw.CUSTOMER_ID =c.CUSTOMER_ID
inner join customer_location cl on cl.CUSTOMER_ID =c.CUSTOMER_ID
inner join customer_location_contact cli on cli.CUSTOMER_LOC_ID= cl.CUSTOMER_LOC_ID
where iw.AR_NUMBER =@ar_number";
        #endregion
          
        #region REGISTER_SAMPLE_NOTIFICATION
        internal static string GET_SAMPLE_DETAILS = @"select im.CUSTOMER_LOT_NO,
                                                    im.NO_OF_CONTAINERS,im.QTY_RECEIEVED,
                                                    (select DISPLAY_NAME from lov_entry where LOV_ENTRY_ID=im.CONTAINER_TYPE  AND LOV_GROUP_ID=2) AS CONTAINER_TYPE,
                                                    (select DISPLAY_NAME from lov_entry where LOV_ENTRY_ID=im.QTY_UOM  AND LOV_GROUP_ID=3) AS UOM,
                                                    (select DISPLAY_NAME from lov_entry where LOV_ENTRY_ID=im.STORAGE_METHOD  AND LOV_GROUP_ID=4) AS STORAGE_METHOD
                                                    from ir_material im
                                                    inner join inward_register iw on iw.INWARD_REGISTER_ID = im.INWARD_REGISTER_ID
                                                    inner join material m on im.MATERIAL_ID = m.MATERIAL_ID
                                                    where iw.AR_NUMBER in ({0});";

        internal static string GET_SAMPLE_REGISTER_NOTIFICATION_TEST = @"select t.TEST_LONG_NAME AS TEST_NAME,imt.IR_TEST_ID
                                                                        from ir_material_test imt
                                                                        inner join  ir_material im on imt.IR_MATERIAL_ID = im.IR_MATERIAL_ID
                                                                        inner join inward_register iw on iw.INWARD_REGISTER_ID = im.INWARD_REGISTER_ID
                                                                        inner join test t on t.TEST_ID = imt.TEST_ID
                                                                        where iw.AR_NUMBER in ({0});";


        internal static string GET_TEST_PRICE_DETAILS = @"select t.TEST_LONG_NAME AS TestName,
                                                        count(IMT.TEST_ID) AS Quantity ,
                                                        IMT.TEST_ID,IMT.INVOICE_PRICE AS UnitPrice,
                                                        IMT.IR_TEST_ID as IrTestId
                                                         from ir_material_test IMT
                                                        INNER JOIN ir_material IM ON IMT.IR_MATERIAL_ID = IM.IR_MATERIAL_ID
                                                        INNER JOIN inward_register IR ON IR.INWARD_REGISTER_ID = IM.INWARD_REGISTER_ID
                                                        inner join test t on t.TEST_ID = IMT.TEST_ID
                                                        where IR.AR_NUMBER=@arnumber  GROUP by IMT.TEST_ID;";

        internal static string GET_PO_NUMBER_AR_DETAILS = @"select INVOICE_AMOUNT as TotalPrice, PO_NUMBER as PoNumber,AR_NUMBER as ArNumber,INWARD_REGISTER_ID as InwardRegisterId from inward_register where AR_NUMBER=@ar_number;";
        #endregion


        #region For NewInwardReg

        public static int DefaultPageSize = 30;

        #region NewInward_Token/Authorize
        // New Inword Reg
        internal static string ValidatePassword = "select * from app_user where USER_ID=@UserID ";
        internal static string GenerateToken = "Insert into APP_TOKEN (User_Id,Issued_On,AuthToken, Expired_On, IsActive) values (@UserId,@IssuedOn,@AuthToken, @ExpiresOn, @IsActive)";
        internal static string GetTokenInfoByEMPID = "select * from APP_TOKEN where User_Id=@UserId and IsActive=@IsActive order by  Issued_On desc";

        internal static string UserDetailsByUserID = @"select  au.id, au.firstname, au.lastname, au.login, au.password, au.role, au.lastlogin, au.active, ur.role as roleid, ur.rolename
                                                         From users au left join userroles ur on ur.userid=au.id where au.login= @login";

        internal static string UpdateTokenByAuthTokenKey = "Update APP_TOKEN set IsActive = @IsActive where AuthToken = @AuthToken and IsActive = 1";
        internal static string InvalidateOldTokensOfUser = "Update APP_TOKEN set IsActive = 0 where User_Id = @User_Id";
        internal static string ValidateTokenKEy = "select * from APP_TOKEN where AuthToken=@AuthToken and IsActive=@IsActive order by Issued_On desc";

        internal static string GetUserByID = @"select  au.id, au.firstname, au.lastname, au.login, au.password, au.role, au.lastlogin, au.active, ur.role as roleid, ur.rolename
                                         From users au left join userroles ur on ur.userid=au.id where au.id= @UserId";

        internal static string CreateUser = @"INSERT INTO users(firstname, lastname, login, password, role, lastlogin, active)
                                                          VALUES(@firstname, @lastname, @login, @password, @role, @lastlogin, @active)";

        internal static string UpdateUserInfo = @"Update users set firstname=@firstname, lastname= @lastname, password=@password where id=@UserID";

        internal static string UpdateUserStatus = @"Update users set active=@active where id=@UserID";

        internal static string UpdateUserPassword = @"Update users set password=@password where id=@UserID";

        internal static string GetUserList = @"select  au.id, au.firstname, au.lastname, au.login, au.password, au.role, au.lastlogin, au.active, ur.role as roleid, ur.rolename
                                                         From users au left join userroles ur on ur.userid=au.id ";

        internal static string GET_ROLES = @"select ROLE_ID As Id, ROLE_DISPLAy_NAME as Value from app_role order by ROLE_DISPLAY_NAME ASC;";
        //@"select * 
        //From user au, app_role ar, app_role_user aru
        //where   au.user_id=aru.user_id and aru.role_id=ar.role_id and au.User_Id=@UserId ";

        internal static string CreateUserRoles = @"INSERT INTO userroles(userid,role,rolename) VALUES(@userid, @role, @rolename)";


        internal static string GetUserRoles = @"Select * from userroles";
        // End New Inword Reg

        internal static string AddUser = "INSERT INTO APP_USER(USER_FIRST_NAME, USER_LAST_NAME, LDAP_USER_DESCRIPTOR, ENCRYPTED_PASSWORD, ENCRYPTED_PIN, LDAP_USER_ID, FAILED_LOGIN_ATTEMPTS, USER_EMAIL_ID, LOCKED, LOCKED_DATE, CREATED_BY, CREATED_DATE, LAST_MODIFIED_BY, LAST_MODIFIED_DATE)VALUES(@USER_FIRST_NAME, @USER_LAST_NAME, @LDAP_USER_DESCRIPTOR, @ENCRYPTED_PASSWORD, @ENCRYPTED_PIN, @LDAP_USER_ID, @FAILED_LOGIN_ATTEMPTS, @USER_EMAIL_ID, @LOCKED, @LOCKED_DATE, @CREATED_BY, @CREATED_DATE, @LAST_MODIFIED_BY, @LAST_MODIFIED_DATE)";
        // changes the isdefault role from query

        #endregion

        internal static string RENEW_USER_TOKEN = "update app_token set EXPIRED_ON = @expired_on where AUTHTOKEN=@authtoken;";

        #region SETTINGS
        internal static string GET_SETTINGS_ALL = "select * from settings";
        internal static string UPDATE_SETTINGS = "Update settings set valuec = @value  Where keyc = @key";
        #endregion

        #region App_Role_UiColumns
        internal static string Get_App_Role_UI_Columns_All = @"select * from app_role_ui_columns;";
        #endregion

        #region grid_queries

        // New Inword Reg
        internal static string IrSampleListGount = "select count(*) from inwardregister where @Condition"; //@Condition


        internal static string IrGridData = @"Select iw.id as InwardRegisterId,iw.ArNumber,iw.ArType, cu.name as Customer, cu.code as CustomerCode,cu.id as CustomerId, 
iw.Sample,iw.Category,iw.SubCategory,
iw.IsRush,iw.SampleCondiation,iw.LotNumber,iw.status,
iw.contanercount as ContainerCount,iw.storagecondiation,iw.resultsremarks, iw.containrtype as ContainerType, iw.Uom, 
iw.CreateDate,iw.LastModifiedDate,iw.count as Quantity,
concat(IFNULL(AU1.firstname,''),' ',IFNULL(AU1.lastname,'')) as LastModifiedBy,
concat(IFNULL(AU.firstname,''),' ',IFNULL(AU.lastname,'')) as CreatedBy,
concat(IFNULL(AU2.firstname,''),' ',IFNULL(AU2.lastname,'')) as Analyst 
from inwardregister iw
LEFT JOIN users AU ON AU.id = iw.CreatedBy
LEFT JOIN users AU1 ON AU1.id = iw.LastModifiedBy
LEFT JOIN users AU2 ON AU2.id = iw.Analyst
LEFT JOIN customer cu ON cu.id = iw.customerid where @Condition 
order by @SortBy @SortOrder LIMIT @perPage OFFSET @pageStart;";




        // End New Inword Reg

        //REGISTER_DATE desc @pageStart,@pageEnd @SortBy @SortOrder


        internal static string GetGridDetails = @";";

        //        internal static string GetGridDetails = @"select concat(cl.STREET_ADDRESS1, IF(ISNULL(cl.STREET_ADDRESS2), concat(' ',cl.STREET_ADDRESS2,','),','), cl.CITY,',',cl.STATE,' - ',cl.ZIP_CODE,',', cl.COUNTRY) as CompleteAddress,
        //cl.STREET_ADDRESS1,cl.STREET_ADDRESS2,cl.CITY,cl.STATE,cl.ZIP_CODE,cl.COUNTRY,
        //clc.CONTACT_PERSON,clc.CONTACT_EMAIL,clc.CONTACT_PHONE,clc.CONTACT_PHONE_EXT,
        //iw.REMARKS,iw.AR_CATEGORY_REMARKS,
        //im.REMARKS AS MATERIAL_REMARKS
        // from inward_register iw
        //inner join ir_material im ON im.INWARD_REGISTER_ID = iw.INWARD_REGISTER_ID and im.IR_Material_ID = (
        //select IR_Material_ID from ir_material where INWARD_REGISTER_ID = iw.INWARD_REGISTER_ID LIMIT 1) 
        //inner join customer c on iw.CUSTOMER_ID = c.CUSTOMER_ID
        //inner join customer_location cl on iw.CUSTOMER_LOCATION_ID = cl.CUSTOMER_LOC_ID
        //inner join customer_location_contact clc on iw.CUSTOMER_CONTACTID = clc.CUSTOMER_LOC_CNCT_ID
        //where iw.AR_NUMBER=@AR_NUMBER;
        //        select CUSTOMER_MAT_CODE1, CONTAINER_TYPE,CUSTOMER_MAT_CODE2, CUSTOMER_LOT_NO, NO_OF_CONTAINERS, QTY_RECEIEVED, QTY_UOM,
        //STORAGE_METHOD from ir_material where INWARD_REGISTER_ID in (select INWARD_REGISTER_ID from inward_register where AR_NUMBER=@AR_NUMBER )
        //;
        //select imr.TEST_STATUS,t.TEST_SHORT_NAME,t.TEST_TYPE,
        //(select CONCAT(USER_FIRST_NAME,' ',USER_LAST_NAME) from app_user where USER_ID =imr.TEST_ASSIGNED_TO_USER ) AS ASSIGN_TO,
        //(select CONCAT(USER_FIRST_NAME,' ',USER_LAST_NAME) from app_user where USER_ID =imr.TEST_RESULTS_ENTERED_USER ) AS RESULTS_SUBMITTED_BY
        // from ir_material_test imr
        //inner join ir_material im on imr.IR_MATERIAL_ID = im.IR_MATERIAL_ID
        //inner join test t on imr.TEST_ID = t.TEST_ID
        //inner join inward_register iw on im.INWARD_REGISTER_ID = iw.INWARD_REGISTER_ID
        //where iw.AR_NUMBER=@AR_NUMBER;";
        #endregion

        #region inward_registry

        internal static string CreateInwardReg = @"   INSERT INTO inwardregister (arnumber, createdate, customerid, sample, othername, artype, category, 
    subcategory, status, isrush, samplecondiation, condiationnotes, notes, createdby,
    analyst, remarks, lotnumber, contanercount, containrtype, count, uom, storagecondiation, resultsremarks)
    VALUES(@arnumber, @createdate, @customerid, @sample, @othername, @artype, @category, 
    @subcategory, @status, @isrush, @samplecondiation, @condiationnotes, @notes, @createdby,
    @analyst, @remarks, @lotnumber, @contanercount, @containrtype, @count, @uom, @storagecondiation, @resultsremarks);
    SELECT LAST_INSERT_ID() AS Id";

        internal static string UpdateInwardReg = @"Update inwardregister Set sample = @sample, othername = @othername, artype = @artype, category = @category, 
      subcategory = @subcategory, status = @status, isrush = @isrush, samplecondiation = @samplecondiation, condiationnotes = @condiationnotes, 
      notes = @notes, analyst = @analyst, assignedby = @assignedby, assigneddate = @assigneddate, lastmodifieddate = @lastmodifieddate, 
      lastmodifiedby = @lastmodifiedby, remarks = @remarks, lotnumber = @lotnumber, contanercount = @contanercount, containrtype = @containrtype, 
      count = @count, uom = @uom, storagecondiation = @storagecondiation, resultsremarks = @resultsremarks where id=@id";

        internal static string SaveAssignAnalystInfo = @"Update inwardregister Set status = @status, analyst = @analyst, assignedby = @assignedby, assigneddate = @assigneddate where id=@id";
        

        internal static string GetInwardRegDetailsByID = @"Select iw.id,iw.id as InwardRegisterId, iw.ArNumber,iw.ArType, cu.name as CustomerName, cu.code as CustomerCode,cu.id as CustomerId, 
iw.Sample,iw.Category,iw.SubCategory,
iw.IsRush,iw.SampleCondiation,iw.LotNumber,iw.status,
iw.contanercount as ContainerCount,iw.storagecondiation,iw.resultsremarks, iw.containrtype as ContainerType, iw.Uom,iw.Analyst, 
iw.CreateDate,iw.CreatedBy, iw.assignedby,iw.assigneddate, iw.LastModifiedDate, iw.LastModifiedBy,iw.count as Quantity,
concat(IFNULL(AU1.firstname,''),' ',IFNULL(AU1.lastname,'')) as LastModifiedByName,
concat(IFNULL(AU.firstname,''),' ',IFNULL(AU.lastname,'')) as CreatedByName,
concat(IFNULL(AU2.firstname,''),' ',IFNULL(AU2.lastname,'')) as AnalystName 
from inwardregister iw
LEFT JOIN users AU ON AU.id = iw.CreatedBy
LEFT JOIN users AU1 ON AU1.id = iw.LastModifiedBy
LEFT JOIN users AU2 ON AU2.id = iw.Analyst
LEFT JOIN customer cu ON cu.id = iw.customerid where iw.id= @InwardRegisterId;";

        #endregion


        internal static string Get_Customer_Next_Sequnece = "usp_get_next_arsequence";

        #region Materials
        internal static string GetAllMaterials = "Select * from Material";
        internal static string GetMaterialByID = "Select * from Material where Material_ID=@ID";

        #endregion

        #region customerDetail
        //internal static string GetAllCustomerDetail = "SELECT * FROM customer c, Customer_location cl, Customer_Location_Contact clc where c.customer_id= cl.customer_id and cl.Customer_Loc_Id= clc.Customer_Loc_Id order by CUSTOMER_SHORT_NAME asc";
        internal static string GetAllCustomerDetail = @"select * from customer Order By name asc;";
        internal static string GetCustomerDetailByName = "SELECT * FROM customer  where name =@Name AND code=@Code";
        internal static string GetCustomerDetailById = "SELECT * FROM customer where id= @ID order by name ASC";
        internal static string AddCustomer = @"INSERT INTO customer(name, code) VALUES(@name, @code); SELECT LAST_INSERT_ID() AS Id";

        #endregion

        #region customer
        internal static string GetAllCustomer = @"SELECT  CUSTOMER_ID,CUSTOMER_IDENTIFIER,CUSTOMER_LONG_NAME ,
                CONCAT(CUSTOMER_SHORT_NAME,' ( ', CUSTOMER_IDENTIFIER,' )') AS CUSTOMER_SHORT_NAME from CUSTOMER order by CUSTOMER_LONG_NAME;";

        internal static string GetCustomerByName = "SELECT * from customer  where CUSTOMER_LONG_NAME =@Name";
        internal static string GetCustomerById = "SELECT * from customer where customer_id= @ID";

        #endregion

        #region Appusers
        internal static string GetUserListByRole_list = @"select au.id, au.firstname, au.lastname, au.login, au.password, au.role, au.lastlogin, au.active, aru.role as roleid, 
                                  aru.rolename, ar.ROLE_ID, ar.ROLE_DISPLAY_NAME From users au, app_role ar, userroles aru
                                       where au.id= aru.userid and aru.role = ar.role_id and ar.role_id in (@roleList)";

        internal static string GetUserListByRole = @"select au.id, au.firstname, au.lastname, au.login, au.password, au.role, au.lastlogin, au.active, aru.role as roleid, 
                                  aru.rolename, ar.ROLE_ID, ar.ROLE_DISPLAY_NAME From users au, app_role ar, userroles aru
                                       where au.id= aru.userid and aru.role = ar.role_id and ar.role_id = @RoleId";

        //internal static string GetUserListByRole = @"select * 
        //                                    From app_user au, app_role ar, app_role_user aru
        //                                    where   au.user_id=aru.user_id and aru.role_id=ar.role_id and ar.role_id=@RoleId";

        #endregion

        #region Codelookup
        internal static string GetAllLookups = "SELECT le.LOV_ENTRY_ID,lg.LOV_GROUP_ID,LOV_GROUP_NAME,DISPLAY_NAME FROM LOV_GROUP lg,LOV_ENTRY le where lg.LOV_GROUP_ID=le.LOV_GROUP_ID";
        internal static string GetLookupByListName = "SELECT le.LOV_ENTRY_ID,lg.LOV_GROUP_ID,LOV_GROUP_NAME,DISPLAY_NAME FROM LOV_GROUP lg,LOV_ENTRY le where lg.LOV_GROUP_ID=le.LOV_GROUP_ID and lg.LOV_GROUP_NAME=@LOV_GROUP_NAME";

        #endregion

        #region TestDetails

        internal static string AddTestDetails = @"INSERT INTO testdetails(inwardregisterid, name) VALUES(@inwardregisterid, @name); SELECT LAST_INSERT_ID() AS Id";
        //internal static string AddTestDetails = @"INSERT INTO testdetails(inwardregisterid, name, result, remarks, resultsby, resultsdate, analyst, assignedby, assigneddate) VALUES(@inwardregisterid, @name, @result, @remarks, @resultsby, @resultsdate, @analyst,@assignedby,@assigneddate); SELECT LAST_INSERT_ID() AS Id";

        internal static string UpdateTestDetails = @"Update testdetails set result = @result, remarks = @remarks, resultsby = @resultsby, resultsdate = @resultsdate, analyst = @analyst, assignedby = @assignedby, assigneddate = @assigneddate where inwardregisterid = @inwardregisterid AND id=@id";

        internal static string UpdateAssignAnalystTestDetails = @"Update testdetails set  analyst = @analyst, assignedby = @assignedby, assigneddate = @assigneddate where inwardregisterid = @inwardregisterid AND id=@id";

        internal static string UpdateEnterResultsTestDetails = @"Update testdetails set result = @result, remarks = @remarks, resultsby = @resultsby, resultsdate = @resultsdate, analyst = @analyst, assignedby = @assignedby, assigneddate = @assigneddate where inwardregisterid = @inwardregisterid AND id=@id";
        

        internal static string GetTestDetailsList = @"Select * from testdetails order by resultsdate desc ";

        internal static string GetTestDetailsByID = @"Select * from testdetails where id= @testdetailID";

        internal static string GetTestDetailsByInwardRegisterId = @"Select * from testdetails where inwardregisterid= @InwardRegisterId";
        #endregion


        #region Test
        internal static string GetAllTests = "SELECT * FROM Test";
        internal static string GetTestByID = "SELECT * FROM Test where TEST_ID=@ID";
        #endregion

        #region Checklist
        internal static string GetChecklistsByMatterandProduct = "SELECT fd.Id, fd.type, fd.FileName, fd.FilePath FROM material_product_checklist mpc JOIN file_detail fd ON fd.Id = mpc.FileId WHERE mpc.MaterialId = @MaterialId AND mpc.ProductId = @ProductId";

        internal static string AddChecklistInward = @"INSERT INTO checklist_inward(FileId, InwardRegID) VALUES(@fileId, @inwardRegId); SELECT LAST_INSERT_ID() AS Id";

        internal static string GetCheckListDetailsByInwardRegisterId = "Select cd.id, cd.FileName, cd.FilePath from checklist_inward ci join file_detail cd on cd.Id= ci.fileId where ci.InwardRegID=@InwardRegID";

        internal static string GetFileListByMatterProductTestId = "SELECT fd.Id, fd.type, fd.FileName, fd.FilePath FROM material_product_test mpt JOIN file_detail fd ON fd.Id = mpt.FileId WHERE mpt.MaterialId = @MaterialId AND mpt.ProductId = @ProductId and mpt.TestId=@TestId";

        internal static string GetFileDetailsByInwRegIdandTestId = "Select fd.Id, fd.type, fd.FileName, fd.FilePath  from test_inward ti join file_detail fd on fd.Id= ti.fileId where ti.InwardRegID=@InwardRegID and ti.TestId=@TestId ";

        internal static string AddTestInward = @"INSERT INTO test_inward(FileId, InwardRegID,TestId,TestDetailId) VALUES(@fileId, @inwardRegId,@testId,@testDetailId); SELECT LAST_INSERT_ID() AS Id";


        internal static string GetTestCheckListDetailsInwardRegisterId = "Select ci.id,ci.InwardRegId,ci.TestDetailId, cd.Type, cd.FileName, cd.FilePath, ci.TestId, ci.FileId from test_inward ci \r\njoin file_detail cd on cd.Id= ci.fileId where ci.InwardRegId=@InwardRegID";
        #endregion

        #endregion

    }
}


