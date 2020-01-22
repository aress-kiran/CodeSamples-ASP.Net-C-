using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Configuration;

/// <summary>
/// Summary description for EmployeePosition
/// </summary>
public class EmployeePosition
{
    #region " Variable Declaration "
    private List<SqlParameter> param = null;
    private string connectionString = string.Empty;
    string connectionStringClient = string.Empty;
    #endregion

    #region " Constructor "
    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeePosition"/> class.
    /// </summary>
    public EmployeePosition()
    {
        connectionString = ConfigurationManager.ConnectionStrings["ESCtMaster"].ToString();
    }
    #endregion

    #region " Public Methods "
    /// <summary>
    /// Gets the employee positions.
    /// </summary>
    /// <returns></returns>
    public List<EmployeePositionData> GetEmployeePositions()
    {
        List<EmployeePositionData> lstEmployeePositionData = new List<EmployeePositionData>();
        EmployeePositionData oEmployeePosition = null;

        SqlDataReader oReader = DBOPS.SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, "GetPositions");

        if (oReader != null)
        {
            while (oReader.Read())
            {
                oEmployeePosition = new EmployeePositionData();
                oEmployeePosition.PositionID = Convert.ToInt32(oReader["PositionID"]);
                oEmployeePosition.Position = Convert.ToString(oReader["Position"]);
                oEmployeePosition.Description = Convert.ToString(oReader["Description"]);
                oEmployeePosition.HouseKeeper = Convert.ToString(oReader["HouseKeeper"]);
                oEmployeePosition.IsDistributed = Convert.ToBoolean(oReader["IsDistributed"]);
                oEmployeePosition.ServiceID = Convert.ToInt32(oReader["Service"]);
                oEmployeePosition.ClassificationID = Convert.ToInt32(oReader["Classification"]);
                oEmployeePosition.Service = GetService(oEmployeePosition.ServiceID);
                oEmployeePosition.Classification = GetClassification(oEmployeePosition.ClassificationID);
                oEmployeePosition.ManagementTraining = Convert.ToBoolean(oReader["ManagementTraining"]);
                oEmployeePosition.IsAnnualCertification = Convert.ToBoolean(oReader["IsAnnualCertification"]);
                oEmployeePosition.IsAssociatedToUser = Convert.ToString(oReader["IsAssignedToUser"]);

                lstEmployeePositionData.Add(oEmployeePosition);
            }
            oReader.Close();
            oReader = null;
        }

        return lstEmployeePositionData;
    }

    /// <summary>
    /// Gets the service.
    /// </summary>
    /// <param name="serviceID">The service identifier.</param>
    /// <returns></returns>
    private string GetService(int serviceID)
    {
        string service = string.Empty;

        switch (serviceID)
        {
            case 1:
                service = Convert.ToString(Enumeration.Service.EVS);
                break;

            case 2:
                service = Convert.ToString(Enumeration.Service.Food);
                break;

            case 3:
                service = Convert.ToString(Enumeration.Service.Linen_Laundry).Replace("_", " ");
                break;

            case 4:
                service = Convert.ToString(Enumeration.Service.Transport);
                break;
        }

        return service;
    }

    /// <summary>
    /// Gets the classification.
    /// </summary>
    /// <param name="classificationID">The classification identifier.</param>
    /// <returns></returns>
    private string GetClassification(int classificationID)
    {
        string classification = string.Empty;

        switch (classificationID)
        {
            case 1:
                classification = Convert.ToString(Enumeration.Classification.Employee);
                break;

            case 2:
                classification = Convert.ToString(Enumeration.Classification.Onsite_Management).Replace("_", " ");
                break;

            case 3:
                classification = Convert.ToString(Enumeration.Classification.Support);
                break;

            case 4:
                classification = Convert.ToString(Enumeration.Classification.DM);
                break;

            case 5:
                classification = Convert.ToString(Enumeration.Classification.RVP);
                break;

            case 6:
                classification = "D.CEO";
                break;

            case 7:
                classification = Convert.ToString(Enumeration.Classification.CEO);
                break;
        }

        return classification;
    }

    /// <summary>
    /// Deletes the employee position.
    /// </summary>
    /// <param name="positionID">The position identifier.</param>
    /// <returns></returns>
    public int DeleteEmployeePosition(int positionID)
    {
        int result = -1;
        object oResult = null;

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@PositionID", positionID));
        
        oResult = DBOPS.SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, "DeleteEmployeePosition", param.ToArray());

        if (oResult != null && Convert.ToString(oResult) != string.Empty)
        {
            result = Convert.ToInt16(oResult);
        }

        return result;
    }

    /// <summary>
    /// Gets the employee position.
    /// </summary>
    /// <param name="positionID">The position identifier.</param>
    /// <returns></returns>
    public EmployeePositionData GetEmployeePosition(int positionID)
    {
        EmployeePositionData oEmployeePositionData = null;

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@PositionID", positionID));

        SqlDataReader oReader = DBOPS.SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, "GetEmployeePostionByID", param.ToArray());

        if (oReader != null)
        {
            while (oReader.Read())
            {
                oEmployeePositionData = new EmployeePositionData();
                oEmployeePositionData.PositionID = Convert.ToInt32(oReader["PositionID"]);
                oEmployeePositionData.Position = Convert.ToString(oReader["Position"]);
                oEmployeePositionData.Description = Convert.ToString(oReader["Description"]);
                oEmployeePositionData.HouseKeepingID = Convert.ToInt32(oReader["HouseKeeperID"]);
                oEmployeePositionData.HouseKeeper = Convert.ToString(oReader["HouseKeeper"]);
                oEmployeePositionData.IsDistributed = Convert.ToBoolean(oReader["IsDistributed"]);
                oEmployeePositionData.ServiceID = Convert.ToInt32(oReader["Service"]);
                oEmployeePositionData.ClassificationID = Convert.ToInt32(oReader["Classification"]);
                oEmployeePositionData.ManagementTraining = Convert.ToBoolean(oReader["ManagementTraining"]);
                oEmployeePositionData.IsAnnualCertification = Convert.ToBoolean(oReader["IsAnnualCertification"]);
            }

            oReader.Close();
            oReader = null;
        }

        return oEmployeePositionData;
    }

    /// <summary>
    /// Gets the curriculum by position.
    /// </summary>
    /// <param name="positionID">The position identifier.</param>
    /// <returns></returns>
    public DataSet GetCurriculumByPosition(int positionID)
    {
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@PositionID", positionID));

        return DBOPS.SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "GetCurriculumByPosition", param.ToArray());
    }

    /// <summary>
    /// Gets the training by curriculum.
    /// </summary>
    /// <param name="curriculumID">The curriculum identifier.</param>
    /// <returns></returns>
    public DataSet GetTrainingByCurriculum(int curriculumID)
    {
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CurriculumID", curriculumID));

        return DBOPS.SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "GetTrainingByCurriculum", param.ToArray());
    }

    /// <summary>
    /// Gets the employee position identifier.
    /// </summary>
    /// <returns></returns>
    public Hashtable GetEmployeePositionID()
    {
        Hashtable htEmployeePostionID = new Hashtable();
      
        SqlDataReader oReader = DBOPS.SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, "GetAllEmployeePostionID");

        while (oReader.Read())
        {
            htEmployeePostionID.Add(htEmployeePostionID.Count + 1, oReader["ep_key"]);
        }

        oReader.Close();
        oReader = null;

        return htEmployeePostionID;
    }

    /// <summary>
    /// Saves the employee position.
    /// </summary>
    /// <param name="employeePostionData">The employee postion data.</param>
    /// <param name="userID">The user identifier.</param>
    /// <returns></returns>
    public int SaveEmployeePosition(EmployeePositionData employeePostionData, int userID)
    {
        int result = 0;
        object oResult = null;

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@ep_key", employeePostionData.PositionID));
        param.Add(new SqlParameter("@ep_name", employeePostionData.Position));
        param.Add(new SqlParameter("@ep_description", employeePostionData.Description));
        param.Add(new SqlParameter("@ep_hk_key", employeePostionData.HouseKeepingID));
        param.Add(new SqlParameter("@ep_service", employeePostionData.ServiceID));
        param.Add(new SqlParameter("@ep_classification", employeePostionData.ClassificationID));
        param.Add(new SqlParameter("@ep_mgt_training", employeePostionData.ManagementTraining));
        param.Add(new SqlParameter("@ep_is_annual_cert", employeePostionData.IsAnnualCertification));
        param.Add(new SqlParameter("@LastUserID", userID));

        oResult = DBOPS.SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, "SaveEmployeePosition", param.ToArray());

        if (oResult != null && Convert.ToString(oResult) != string.Empty)
        {
            result = Convert.ToInt32(oResult);
        }

        return result;
    }

    /// <summary>
    /// Saves the curriculum.
    /// </summary>
    /// <param name="positionID">The position identifier.</param>
    /// <param name="curriculumIds">The curriculum ids.</param>
    /// <returns></returns>
    public int SaveCurriculum(int positionID, string curriculumIds)
    {
        int result = 0;
        object oResult = null;

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@PositionID", positionID));
        param.Add(new SqlParameter("@CurriculumIds", curriculumIds));

        oResult = DBOPS.SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, "SaveCurriculum", param.ToArray());

        if (oResult != null && Convert.ToString(oResult) != string.Empty)
        {
            result = Convert.ToInt32(oResult);
        }

        return result;
    }

    /// <summary>
    /// Gets the house keeping.
    /// </summary>
    /// <returns></returns>
    public List<HouseKeepData> GetHouseKeeping()
    {
        List<HouseKeepData> lstHouseKeepDataList = new List<HouseKeepData>();
        HouseKeepData oHouseKeepData = null; 

        SqlDataReader oReader = DBOPS.SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, "GetHouseKeepers");

        if (oReader != null)
        {
            while (oReader.Read())
            {
                oHouseKeepData = new HouseKeepData();
                oHouseKeepData.HouseKeepID = Convert.ToInt32(oReader["hk_key"]);
                oHouseKeepData.Type = Convert.ToInt32(oReader["hk_type"]);
                oHouseKeepData.Description = Convert.ToString(oReader["hk_description"]);

                lstHouseKeepDataList.Add(oHouseKeepData);
            }
            oReader.Close();
            oReader = null;
        }

        return lstHouseKeepDataList;
    }

    /// <summary>
    /// Gets the positions to publish.
    /// </summary>
    /// <returns></returns>
    public List<EmployeePositionData> GetPositionsToPublish()
    {
        List<EmployeePositionData> lstPositions = new List<EmployeePositionData>();
        EmployeePositionData oEmployeePosition = null;

        SqlDataReader oReader = DBOPS.SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, "GetPropagatePosition");

        if (oReader != null)
        {
            while (oReader.Read())
            {
                oEmployeePosition = new EmployeePositionData();

                oEmployeePosition.PositionID = Convert.ToInt32(oReader["PositionID"]);
                oEmployeePosition.Position = Convert.ToString(oReader["Position"]);
                oEmployeePosition.ServiceID = Convert.ToInt32(oReader["ServiceID"]);
                oEmployeePosition.ClassificationID = Convert.ToInt32(oReader["ClassificationID"]);
                oEmployeePosition.IsDistributed = Convert.ToBoolean(oReader["IsDistribute"]);
                oEmployeePosition.Service = GetService(oEmployeePosition.ServiceID);
                oEmployeePosition.Classification = GetClassification(oEmployeePosition.ClassificationID);

                lstPositions.Add(oEmployeePosition);
            }

            oReader.Close();
            oReader = null;
        }

        return lstPositions;
    }

    /// <summary>
    /// Publishes the employee positions.
    /// </summary>
    /// <param name="positionIDs">The position i ds.</param>
    /// <param name="selectedAppToPublish">The selected application to publish.</param>
    /// <param name="lastUserID">The last user identifier.</param>
    /// <returns></returns>
    public string PublishEmployeePositions(string positionIDs, int lastUserID, string transactionDate)
    {
        string result = string.Empty;

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@positionIDs", positionIDs));
        param.Add(new SqlParameter("@lastUserID", lastUserID));
        param.Add(new SqlParameter("@TransactionDate", transactionDate));

        SqlDataReader oReader = DBOPS.SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, "PublishEmployeePositions", param.ToArray());

        if (oReader != null && Convert.ToString(oReader) != string.Empty)
        {
            if (oReader.Read())
            {
                result = Convert.ToString(oReader["Published"]);
            }
            oReader.Close();
            oReader = null;
        }

        return result;
    }

    /// <summary>
    /// Gets the employees to migrate.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="facility">The facility.</param>
    /// <returns></returns>
    public List<EmployeePositionData> GetEmployeesToMigrate(int clientId, int facility)
    {
        List<EmployeePositionData> lstEmployees = new List<EmployeePositionData>();
        EmployeePositionData oEmployee = null;
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@fa_key", facility));
        param.Add(new SqlParameter("@userID", clientId));

        SqlDataReader oReader = DBOPS.SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, "GetEmployeesToMigrate", param.ToArray());

        if (oReader != null)
        {
            while (oReader.Read())
            {
                oEmployee = new EmployeePositionData();

                oEmployee.EmployeeID = Convert.ToInt32(oReader["em_key"]);
                oEmployee.Employee = Convert.ToString(oReader["Employee"]);

                lstEmployees.Add(oEmployee);
            }

            oReader.Close();
            oReader = null;
        }

        return lstEmployees;
    }

    /// <summary>
    /// Gets all supervisors.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="facility">The facility.</param>
    /// <param name="employee">The employee.</param>
    /// <returns></returns>
    public List<EmployeePositionData> GetAllSupervisors(int clientId, int facility, int employee)
    {
        WsFacility wsfacility = new WsFacility();
        string connectionstringweb = wsfacility.GetConnectionString(clientId);

        List<EmployeePositionData> lstEmployees = new List<EmployeePositionData>();
        EmployeePositionData oSupervisors = null;
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@fa_key", facility));

        SqlDataReader oReader = DBOPS.SqlHelper.ExecuteReader(connectionstringweb, CommandType.StoredProcedure, "GetSupervisors", param.ToArray());

        if (oReader != null)
        {
            while (oReader.Read())
            {
                oSupervisors = new EmployeePositionData();

                oSupervisors.EmployeeID = Convert.ToInt32(oReader["em_key"]);
                oSupervisors.Employee = Convert.ToString(oReader["Supervisor"]);
                oSupervisors.IsActive = Convert.ToBoolean(oReader["em_active"]); 

                lstEmployees.Add(oSupervisors);
            }

            oReader.Close();
            oReader = null;
        }

        return lstEmployees;
    }

    /// <summary>
    /// Checks the is supervisor with open history.
    /// </summary>
    /// <param name="sourceClientId">The source client identifier.</param>
    /// <param name="destinationClientId">The destination client identifier.</param>
    /// <param name="employeeKey">The employee key.</param>
    /// <returns></returns>
    public string CheckIsSupervisorWithOpenHistory(int sourceClientId, int destinationClientId, int employeeKey)
    {
        string result = string.Empty;
        object oResult = null;

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@em_key", employeeKey));
        param.Add(new SqlParameter("@SourceClientID", sourceClientId));
        param.Add(new SqlParameter("@DestinationClientID", destinationClientId));
        oResult = DBOPS.SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, "CheckIsSupervisorWithOpenHistory", param.ToArray());

        if (oResult != null && Convert.ToString(oResult) != string.Empty)
        {
            result = Convert.ToString(oResult);
        }

        return result;
    }

    /// <summary>
    /// Gets the client connection string.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <returns></returns>
    public string GetClientConnectionString(int clientId)
    {
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@clientID", clientId));      
        connectionStringClient = Convert.ToString(DBOPS.SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, "GetClientConnectionString", param.ToArray()));
        return connectionStringClient;
    }

    /// <summary>
    /// Copies the employee position.
    /// </summary>
    /// <param name="positionIdToCopy">The position identifier to copy.</param>
    /// <param name="newPositionName">New name of the position.</param>
    /// <param name="lastUserId">The last user identifier.</param>
    /// <returns></returns>
    public int CopyEmployeePosition(int positionIdToCopy, string newPositionName, int lastUserId)
    {
        int result = 0;
        object oResult = null;

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@positionId", positionIdToCopy));
        param.Add(new SqlParameter("@newPosition", newPositionName));
        param.Add(new SqlParameter("@lastUserId", lastUserId));

        oResult = DBOPS.SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, "CopyEmployeePosition", param.ToArray());

        if (oResult != null && Convert.ToString(oResult) != string.Empty)
        {
            result = Convert.ToInt32(oResult);
        }

        return result;
    }

    /// <summary>
    /// Transfers the employee training.
    /// </summary>
    /// <param name="sourceClientId">The source client identifier.</param>
    /// <param name="sourceFacilityKey">The source facility key.</param>
    /// <param name="destinationClientId">The destination client identifier.</param>
    /// <param name="destinationFacilityKey">The destination facility key.</param>
    /// <param name="employeeKey">The employee key.</param>
    /// <param name="action">The action.</param>
    /// <param name="newSupervisorId">The new supervisor identifier.</param>
    /// <param name="isTransferResp">The is transfer resp.</param>
    /// <param name="openDataSource">The open data source.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns></returns>
    public int TransferEmployeesTrainings(int sourceClientId, int SourceFacilityId, int destinationClientId, int destinationFacilityId, int sourceEmployeeId, string action, int newSupervisorId, int isTransferResp, string openDataSourceQuery, int loginUserId, string transactionDate)
    {
        int result = 0;
        object oResult = null;

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@SourceClientId", sourceClientId));
        param.Add(new SqlParameter("@SourceFacilityId", SourceFacilityId));
        param.Add(new SqlParameter("@DestinationClientId", destinationClientId));
        param.Add(new SqlParameter("@DestinationFacilityId", destinationFacilityId));
        param.Add(new SqlParameter("@SourceEmployeeId", sourceEmployeeId));
        param.Add(new SqlParameter("@Action", action));
        param.Add(new SqlParameter("@NewSupervisorId", newSupervisorId));
        param.Add(new SqlParameter("@IsTransferResp", isTransferResp));
        param.Add(new SqlParameter("@OpenDataSourceQuery", openDataSourceQuery));
        param.Add(new SqlParameter("@LoginUserId", loginUserId));
        param.Add(new SqlParameter("@TransactionDate", transactionDate));

        oResult = DBOPS.SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, "TransferEmployeesTrainings", param.ToArray());

        if (oResult != null && Convert.ToString(oResult) != string.Empty)
        {
            result = Convert.ToInt32(oResult);
        }

        return result;
    }

    /// <summary>
    /// Gets the facility master user.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="facility">The facility.</param>
    /// <returns></returns>
    public List<EmployeePositionData> GetFacilityMasterUser(int clientId, int facility)
    {
        List<EmployeePositionData> lstEmployees = new List<EmployeePositionData>();
        EmployeePositionData oEmployee = null;
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@fa_key", facility));
        param.Add(new SqlParameter("@userID", clientId));

        SqlDataReader oReader = DBOPS.SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, "GetFacilityMasterUser", param.ToArray());

        if (oReader != null)
        {
            while (oReader.Read())
            {
                oEmployee = new EmployeePositionData();

                oEmployee.EmployeeID = Convert.ToInt32(oReader["em_key"]);
                oEmployee.Employee = Convert.ToString(oReader["Employee"]);
                oEmployee.Facility = Convert.ToInt32(oReader["fa_key"]);
                oEmployee.EmployeeEmailId = Convert.ToString(oReader["cu_email"]);

                lstEmployees.Add(oEmployee);
            }

            oReader.Close();
            oReader = null;
        }

        return lstEmployees;
    }

    /// <summary>
    /// Gets the positions based on access.
    /// </summary>
    /// <param name="accessType">Type of the access.</param>
    /// <returns></returns>
    public List<EmployeePositionData> GetPositionsBasedOnAccess(int accessType)
    {
        List<EmployeePositionData> lstPositions = new List<EmployeePositionData>();
        EmployeePositionData oPosition = null;
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@accessType", accessType));

        SqlDataReader oReader = DBOPS.SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, "GetPositionsBasedOnAccess", param.ToArray());

        if (oReader != null)
        {
            while (oReader.Read())
            {
                oPosition = new EmployeePositionData();
                oPosition.PositionID = Convert.ToInt32(oReader["PositionID"]);
                oPosition.Position = Convert.ToString(oReader["Position"]);
                lstPositions.Add(oPosition);
            }

            oReader.Close();
            oReader = null;
        }

        return lstPositions;
    }   

    /// <summary>
    /// Gets the management users.
    /// </summary>
    /// <returns></returns>
    public List<EmployeePositionData> GetManagementUsers(int clientId, int facility)
    {
        List<EmployeePositionData> lstEmployees = new List<EmployeePositionData>();
        EmployeePositionData oEmployee = null;
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@fa_key", facility));
        param.Add(new SqlParameter("@userID", clientId));

        SqlDataReader oReader = DBOPS.SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, "GetEmployeesToMigrate", param.ToArray());

        if (oReader != null)
        {
            while (oReader.Read())
            {
                oEmployee = new EmployeePositionData();

                oEmployee.EmployeeID = Convert.ToInt32(oReader["USER_ID"]);
                oEmployee.Employee = Convert.ToString(oReader["UserName"]);

                lstEmployees.Add(oEmployee);
            }

            oReader.Close();
            oReader = null;
        }

        return lstEmployees;
    }

    /// <summary>
    /// Get Position By Curriculum
    /// </summary>
    /// <returns></returns>
    public List<EmployeePositionData> GetPositionByCurriculum(Int32 CurriculumId)
    {
        List<EmployeePositionData> lstEmployeePositionData = new List<EmployeePositionData>();
        EmployeePositionData oEmployeePosition = null;

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CurriculumId", CurriculumId));

        SqlDataReader oReader = DBOPS.SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, "GetPositionByCurriculum", param.ToArray());

        if (oReader != null)
        {
            while (oReader.Read())
            {
                oEmployeePosition = new EmployeePositionData();
                oEmployeePosition.PositionID = Convert.ToInt32(oReader["PositionID"]);

                lstEmployeePositionData.Add(oEmployeePosition);
            }
            oReader.Close();
            oReader = null;
        }

        return lstEmployeePositionData;
    }
    #endregion
}