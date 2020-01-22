using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Admin_AddEditEmployeePosition : Page
{
    #region "Variable Declarations"
    private Hashtable htEmployeePostion = new Hashtable();
    List<HouseKeepData> lstHouseKeeping = null;
    WsEmployeePosition wsEmployeePosition = null;
    DataSet dsCurriculum = new DataSet();
    DataSet dsTraining = new DataSet();
    #endregion

    #region "Page Events"
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        SetVariables();
        LoadNavigation();

        if (!IsPostBack)
        {
            LoadHouseKeeping();
            LoadEmployeePosition();
            LoadGrid(false);
        }
    }
    #endregion

    #region "Control Events"
    /// <summary>
    /// Save New Employee Position
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveNew_Click(object sender, ImageClickEventArgs e)
    {
        SaveNewClick();
    }

    /// <summary>
    /// Save Employee Position
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        SaveClick();
    }

    /// <summary>
    /// Close Add Edit Employee Position Page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, ImageClickEventArgs e)
    {
        CloseClick();
    }

    /// <summary>
    /// Get Previous record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrevious_Click(object sender, ImageClickEventArgs e)
    {
        PreviousClick();
    }

    /// <summary>
    /// Get Next Record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNext_Click(object sender, ImageClickEventArgs e)
    {
        NextClick();
    }

    /// <summary>
    /// Get first record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnFirst_Click(object sender, ImageClickEventArgs e)
    {
        FirstClick();
    }

    /// <summary>
    /// Get Last record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLast_Click(object sender, ImageClickEventArgs e)
    {
        LastClick();
    }

    /// <summary>
    /// Undo previous activity
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUndo_Click(object sender, ImageClickEventArgs e)
    {
        UndoClick();
    }

    /// <summary>
    /// Copy and New Curriculum
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCopyNew_Click(object sender, ImageClickEventArgs e)
    {
        CopyNewPosition();
    }
    #endregion

    #region "Grid Events"
    /// <summary>
    /// Grid Curriculum Need data Source
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdCurriculum_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        GridNeedDataSource();
    }

    /// <summary>
    /// Grid Curriculum Detail table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdCurriculum_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataitem = (GridDataItem)e.DetailTableView.ParentItem;
        int tcKey = Convert.ToInt32(dataitem.GetDataKeyValue("tcc_key").ToString());

        wsEmployeePosition = new WsEmployeePosition();
        dsTraining = wsEmployeePosition.GetTrainingByCurriculum(tcKey);
        wsEmployeePosition = null;

        e.DetailTableView.DataSource = dsTraining;
    }

    /// <summary>
    /// Grid Curriculum item data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdCurriculum_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridCurriculumItemDataBound(sender, e);
    }

    /// <summary>
    /// Grid curriculum prerender
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdCurriculum_PreRender(object sender, EventArgs e)
    {
        GridPreRender();
    }

    #endregion

    #region "Private Methods"

    #region "Events Methods"
    /// <summary>
    /// Performs Save and New operations.
    /// </summary>
    private void SaveNewClick()
    {
        if (hdnFormModified.Value == "1")
        {
            int result = SaveEmployeePosition();

            if (result > 0)
            {
                SaveCurriculum(result);
                Session[SESS_ACCESS_RIGHT] = null;
                ClearPage();
            }
        }
        else
        {
            ClearPage();
            lblMessage.Text = Messages.DisplayMessage(MSG01, false);
        }
    }

    /// <summary>
    /// Performs Save operation.
    /// </summary>
    private void SaveClick()
    {
        bool isEmpPositionSaved = false;
        int result = 0;
        if (hdnFormModified.Value == "1")
        {
             result = SaveEmployeePosition();

            if (result > 0)
            {
                SaveCurriculum(result);
                isEmpPositionSaved = true;
            }
        }
        else
        {
            isEmpPositionSaved = true;
        }

        if (isEmpPositionSaved)
        {
            switch (hdnLastButtonClicked.Value)
            {
                case "Save":
                    Session[SESS_MESSAGE] = Messages.DisplayMessage(MSG01, false);
                    Session[SESS_IS_CALLED_FROM_PUBLISH] = hdnPublishSession.Value;
                    Session[SESS_POSITION_ID] = result;
                    Response.Redirect(hdnPublishSession.Value.Equals("2") ? PAGE_PUBLISH_DATA : PAGE_ADD_EDIT_PAGE_EMPLOYEE_POSITION);
                    break;

                case "First":
                    FirstClick();
                    break;

                case "Previous":
                    PreviousClick();
                    break;

                case "Next":
                    NextClick();
                    break;

                case "Last":
                    LastClick();
                    break;

                default:
                    MasterPage master = (MasterPage)Page.Master;
                    HiddenField hdnMasterVar = (HiddenField)master.FindControl("hdnRibbonButtonToClick");
                    hdnMasterVar.Value = hdnLastButtonClicked.Value;
                    Label lbl = (Label)master.FindControl("lblMasterSuccess");
                    lbl.Text = MSG01;
                    string showPopup = "$(function () { " + "LoadPopup('popupSuccess');" + "});";
                    Page.RegisterStartupScript("ShowMasterPopup", "<script type='text/javascript'>" + showPopup + "</script>");
                    break;
            }
        }
    }

    /// <summary>
    /// Close the form and return to List View.
    /// </summary>
    private void CloseClick()
    {
        Session[SESS_IS_CALLED_FROM_PUBLISH] = hdnPublishSession.Value;
        Response.Redirect(hdnPublishSession.Value.Equals("2") ? PAGE_PUBLISH_DATA : PAGE_EMPLOYEE_POSITIONS);
        hdnPublishSession.Value = string.Empty;
    }

    /// <summary>
    /// Performs Navigation operation for Previous.
    /// </summary>
    private void PreviousClick()
    {
        Navigate(Convert.ToInt32(hdnCurrentKey.Value) - 1);
    }

    /// <summary>
    /// Performs Navigation operation for Next.
    /// </summary>
    private void NextClick()
    {
        Navigate(Convert.ToInt32(hdnCurrentKey.Value) + 1);
    }

    /// <summary>
    /// Navigates to First record.
    /// </summary>
    private void FirstClick()
    {
        Navigate(1);
    }

    /// <summary>
    /// Navigates to Last record.
    /// </summary>
    private void LastClick()
    {
        Navigate(htEmployeePostion.Count);
    }

    /// <summary>
    /// Resets the form or loads the last ID.
    /// </summary>
    private void UndoClick()
    {
        Response.Redirect(PAGE_ADD_EDIT_PAGE_EMPLOYEE_POSITION);
        hdnPublishSession.Value = string.Empty;
    }

    /// <summary>
    /// Load Curriculum Grid
    /// </summary>
    /// <param name="rebind"></param>
    private void LoadGrid(bool rebind)
    {
        LoadCurriculumByPosition();
        grdCurriculum.DataSource = dsCurriculum;
        if (rebind) grdCurriculum.Rebind();
    }

    /// <summary>
    /// Save Curriculum
    /// </summary>
    /// <param name="PositionID"></param>
    private void SaveCurriculum(int PositionID)
    {
        wsEmployeePosition = new WsEmployeePosition();
        string curriculumIds = string.Empty;

        foreach (GridDataItem item in grdCurriculum.MasterTableView.Items)
        {
            string CurriculumID = Convert.ToString(item.GetDataKeyValue("tcc_key").ToString());
            bool chkIsSelected = ((CheckBox)item.FindControl("chkIsSelected")).Checked;

            if (chkIsSelected && !string.IsNullOrEmpty(CurriculumID))
            {
                curriculumIds = !string.IsNullOrEmpty(curriculumIds) ? string.Concat(curriculumIds, CurriculumID, ",") : string.Concat(CurriculumID, ",");

            }
        }

        int result = wsEmployeePosition.SaveCurriculum(PositionID, curriculumIds);
    }

    /// <summary>
    /// Grid Curriculum Item DataBound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GridCurriculumItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem oItem = (GridDataItem)e.Item;
            CheckBox checkbox = (CheckBox)oItem.FindControl("chkIsSelected");

            if (checkbox != null)
            {
                checkbox.Attributes.Remove("onclick");
                checkbox.Attributes.Add("onclick", "EnableCurriculumCheckChanged();");
            }
        }
    }

    /// <summary>
    /// Grid PreRender
    /// </summary>
    private void GridPreRender()
    {
        HideExpandGridColumn(grdCurriculum.MasterTableView);
    }

    /// <summary>
    /// Grid Need DataSource
    /// </summary>
    private void GridNeedDataSource()
    {
        LoadGrid(false);
    }

    /// <summary>
    /// Copy New Curriculum
    /// </summary>
    private void CopyNewPosition()
    {
        wsEmployeePosition = new WsEmployeePosition();
        int result = 0;
        result = wsEmployeePosition.CopyEmployeePosition(Convert.ToInt32(hdnEmpPositionID.Value), Convert.ToString(txtPosition.Text.Trim() + "_Copy"), Convert.ToInt32(hdnUserID.Value));

        if (result > 0)
        {
            hdnEmpPositionID.Value = Convert.ToString(result);
            hdnCurrentKey.Value = "0";

            LoadHouseKeeping();
            LoadEmployeePosition();
            LoadGrid(false);
            LoadNavigation();
        }

        switch (result)
        {
            case -1:
                lblMessage.Text = Messages.DisplayMessage(MSG06, true);
                break;

            case 0:
                lblMessage.Text = Messages.DisplayMessage(MSG05, true);
                break;

            default:
                lblMessage.Text = Messages.DisplayMessage(MSG04, false);
                break;
        }

        wsEmployeePosition = null;
    }
    #endregion

    #region "Other Methods"

    /// <summary>
    /// Sets the variables.
    /// </summary>
    private void SetVariables()
    {
        if (!IsPostBack)
        {
            GetSessions();
        }

        ShowHideCopyNew();
    }

    /// <summary>
    /// Reads all the sessions and sets values to variables.
    /// </summary>
    private void GetSessions()
    {
        PStSession.CheckMasterUserSession();

        if (PStSession.IsSessionPresent(SESS_USER_ID))
        {
            hdnUserID.Value = Session[SESS_USER_ID].ToString();
        }
        
        if (PStSession.IsSessionPresent(SESS_POSITION_ID))
        {
            hdnEmpPositionID.Value = Session[SESS_POSITION_ID].ToString();
            Session.Remove(SESS_POSITION_ID);
            lblMessage.Text = PStSession.IsSessionPresent(SESS_MESSAGE) ? Session[SESS_MESSAGE].ToString() : "";
            Session.Remove(SESS_MESSAGE);
        }

        if (PStSession.IsSessionPresent(SESS_IS_CALLED_FROM_PUBLISH))
        {
            hdnPublishSession.Value = Session[SESS_IS_CALLED_FROM_PUBLISH].ToString();
            Session.Remove(SESS_IS_CALLED_FROM_PUBLISH);
        }

        PStSession.ClearFindSessions();
    }

    /// <summary>
    /// Loads the house keeping data into Dropdown.
    /// </summary>
    private void LoadHouseKeeping()
    {
        wsEmployeePosition = new WsEmployeePosition();
        lstHouseKeeping = wsEmployeePosition.GetHouseKeeping();

        ddlHouseKeeping.DataSource = lstHouseKeeping;
        ddlHouseKeeping.DataTextField = "Description";
        ddlHouseKeeping.DataValueField = "HouseKeepID";
        ddlHouseKeeping.DataBind();

        wsEmployeePosition = null;
    }

    /// <summary>
    /// Loads the Employee Position and grid with records.
    /// </summary>
    private void LoadEmployeePosition()
    {
        wsEmployeePosition = new WsEmployeePosition();
        EmployeePositionData oEmployeePositionData = wsEmployeePosition.GetEmployeePosition(Convert.ToInt32(hdnEmpPositionID.Value));

        if (oEmployeePositionData != null)
        {
            hdnLastEmpPositionID.Value = hdnEmpPositionID.Value;
            txtPosition.Text = oEmployeePositionData.Position;
            txtDescription.Text = oEmployeePositionData.Description;
            ddlHouseKeeping.SelectedValue = Convert.ToString(oEmployeePositionData.HouseKeepingID);
            ddlService.SelectedValue = Convert.ToString(oEmployeePositionData.ServiceID);
            ddlClassification.SelectedValue = Convert.ToString(oEmployeePositionData.ClassificationID);
            chkMgtTraining.Checked = Convert.ToBoolean(oEmployeePositionData.ManagementTraining);
            chkAnnualCertification.Checked = Convert.ToBoolean(oEmployeePositionData.IsAnnualCertification);

            txtPosition.Enabled = oEmployeePositionData.IsDistributed ? false : true;
            hdnIsPublished.Value = Convert.ToString(oEmployeePositionData.IsDistributed);
        }

        wsEmployeePosition = null;
    }

    /// <summary>
    /// Load Curriculum By Position
    /// </summary>
    private void LoadCurriculumByPosition()
    {
        wsEmployeePosition = new WsEmployeePosition();
        dsCurriculum = wsEmployeePosition.GetCurriculumByPosition(Convert.ToInt32(hdnEmpPositionID.Value));
        wsEmployeePosition = null;
    }

    /// <summary>
    /// Saves the EmployeePosition and its Access Rights.
    /// </summary>
    /// <returns></returns>
    private int SaveEmployeePosition()
    {
        int result = 0;
        wsEmployeePosition = new WsEmployeePosition();
        EmployeePositionData oEmployeePositionData = ReadEmployeePositionData();

        if (!string.IsNullOrEmpty(oEmployeePositionData.Position))
        {
            result = wsEmployeePosition.SaveEmployeePostion(oEmployeePositionData, Convert.ToInt32(hdnUserID.Value));
        }

        switch (result)
        {
            case -1:
                lblMessage.Text = Messages.DisplayMessage(MSG03, true);
                break;

            case 0:
                lblMessage.Text = Messages.DisplayMessage(MSG02, true);
                break;

            default:
                lblMessage.Text = Messages.DisplayMessage(MSG01, false);
                break;
        }

        wsEmployeePosition = null;
        return result;
    }

    /// <summary>
    /// Reads the page data and return the EmployeePosition Data object.
    /// </summary>
    /// <returns></returns>
    private EmployeePositionData ReadEmployeePositionData()
    {
        EmployeePositionData oEmployeePositionData = new EmployeePositionData();

        oEmployeePositionData.PositionID = Convert.ToInt32(hdnEmpPositionID.Value);
        oEmployeePositionData.Position = txtPosition.Text.Trim();
        oEmployeePositionData.Description = txtDescription.Text.Trim();
        oEmployeePositionData.HouseKeepingID = Convert.ToInt32(ddlHouseKeeping.SelectedValue);
        oEmployeePositionData.ServiceID = Convert.ToInt32(ddlService.SelectedValue);
        oEmployeePositionData.ClassificationID = Convert.ToInt32(ddlClassification.SelectedValue);
        oEmployeePositionData.ManagementTraining = chkMgtTraining.Checked;
        oEmployeePositionData.IsAnnualCertification = chkAnnualCertification.Checked;

        return oEmployeePositionData;
    }

    /// <summary>
    /// Clear page controls, sessions and variables.
    /// </summary>
    private void ClearPage()
    {
        hdnEmpPositionID.Value = "0";
        ddlHouseKeeping.SelectedIndex = 1;
        ddlService.SelectedValue = "0";
        ddlClassification.SelectedValue = "0";
        chkMgtTraining.Checked = false;
        chkAnnualCertification.Checked = false;

        txtPosition.Text = string.Empty;
        txtDescription.Text = string.Empty;
        hdnIsPublished.Value = "0";
        EnableDisableNavigation(0);
    }

    /// <summary>
    /// Hide Expand Grid Column
    /// </summary>
    /// <param name="tableView"></param>
    private void HideExpandGridColumn(GridTableView tableView)
    {
        GridItem[] nestedViewItems = tableView.GetItems(GridItemType.NestedView);

        foreach (GridNestedViewItem nestedViewItem in nestedViewItems)
        {
            foreach (GridTableView nestedView in nestedViewItem.NestedTableViews)
            {
                if (nestedView.Items.Count == 0)
                {
                    TableCell cell = nestedView.ParentItem["ExpandColumn"];
                    cell.Controls[0].Visible = false;
                    cell.Text = " ";
                    nestedViewItem.Visible = false;
                }

                if (nestedView.HasDetailTables)
                {
                    HideExpandGridColumn(nestedView);
                }
            }
        }
    }

    #endregion

    #region "Navigation"

    /// <summary>
    /// Gets the list of EmployeePostion for edit mode, and loads the current position.
    /// </summary>
    private void LoadNavigation()
    {
        wsEmployeePosition = new WsEmployeePosition();
        htEmployeePostion = wsEmployeePosition.GetEmployeePositionID();
        wsEmployeePosition = null;

        if (Convert.ToInt32(hdnCurrentKey.Value) == 0)
        {
            hdnCurrentKey.Value = Hash.GetHashKey(htEmployeePostion, Convert.ToInt32(hdnEmpPositionID.Value)).ToString();
        }

        EnableDisableNavigation(Convert.ToInt32(hdnCurrentKey.Value));
    }

    /// <summary>
    /// Enables Disables Navigation buttons.
    /// </summary>
    /// <param name="key">Current Hash Key</param>
    private void EnableDisableNavigation(int key)
    {

        lblHeader.Text = "Edit Employee Position";
        lblRecordCountTop.Text = key + " of " + htEmployeePostion.Count;
        lblRecordCountBottom.Text = key + " of " + htEmployeePostion.Count;

        btnNextTop.ImageUrl = btnNextBottom.ImageUrl = IMG_NEXT;
        btnPreviousTop.ImageUrl = btnPreviousBottom.ImageUrl = IMG_PREVIOUS;
        btnLastTop.ImageUrl = btnLastBottom.ImageUrl = IMG_LAST;
        btnFirstTop.ImageUrl = btnFirstBottom.ImageUrl = IMG_FIRST;

        btnNextTop.Enabled = btnNextBottom.Enabled = true;
        btnPreviousTop.Enabled = btnPreviousBottom.Enabled = true;
        btnLastTop.Enabled = btnLastBottom.Enabled = true;
        btnFirstTop.Enabled = btnFirstBottom.Enabled = true;

        if (key == 0)
        {
            lblHeader.Text = "Add Employee Position";
            lblRecordCountTop.Text = "0 of 0";
            lblRecordCountBottom.Text = "0 of 0";

            btnPreviousTop.ImageUrl = btnPreviousBottom.ImageUrl = IMG_PREVIOUS_DISABLE;
            btnNextTop.ImageUrl = btnNextBottom.ImageUrl = IMG_NEXT_DISABLE;
            btnLastTop.ImageUrl = btnLastBottom.ImageUrl = IMG_LAST_DISABLE;
            btnFirstTop.ImageUrl = btnFirstBottom.ImageUrl = IMG_FIRST_DISABLE;

            btnPreviousTop.Enabled = btnPreviousBottom.Enabled = false;
            btnNextTop.Enabled = btnNextBottom.Enabled = false;
            btnLastTop.Enabled = btnLastBottom.Enabled = false;
            btnFirstTop.Enabled = btnFirstBottom.Enabled = false;

            lblRecordCountTop.Enabled = lblRecordCountBottom.Enabled = false;
        }
        else if (key == 1 && key == htEmployeePostion.Count)
        {
            btnPreviousTop.ImageUrl = btnPreviousBottom.ImageUrl = IMG_PREVIOUS_DISABLE;
            btnNextTop.ImageUrl = btnNextBottom.ImageUrl = IMG_NEXT_DISABLE;
            btnLastTop.ImageUrl = btnLastBottom.ImageUrl = IMG_LAST_DISABLE;
            btnFirstTop.ImageUrl = btnFirstBottom.ImageUrl = IMG_FIRST_DISABLE;

            btnPreviousTop.Enabled = btnPreviousBottom.Enabled = false;
            btnNextTop.Enabled = btnNextBottom.Enabled = false;
            btnLastTop.Enabled = btnLastBottom.Enabled = false;
            btnFirstTop.Enabled = btnFirstBottom.Enabled = false;
        }
        else if (key == 1)
        {
            btnPreviousTop.ImageUrl = btnPreviousBottom.ImageUrl = IMG_PREVIOUS_DISABLE;
            btnFirstTop.ImageUrl = btnFirstBottom.ImageUrl = IMG_FIRST_DISABLE;

            btnPreviousTop.Enabled = btnPreviousBottom.Enabled = false;
            btnFirstTop.Enabled = btnFirstBottom.Enabled = false;
        }
        else if (key == htEmployeePostion.Count)
        {
            btnNextTop.ImageUrl = btnNextBottom.ImageUrl = IMG_NEXT_DISABLE;
            btnLastTop.ImageUrl = btnLastBottom.ImageUrl = IMG_LAST_DISABLE;

            btnNextTop.Enabled = btnNextBottom.Enabled = false;
            btnLastTop.Enabled = btnLastBottom.Enabled = false;
        }
    }

    /// <summary>
    /// Navigates the record to Left or Right.
    /// </summary>
    /// <param name="key">Key for Hash Table</param>
    private void Navigate(int key)
    {
        hdnEmpPositionID.Value = htEmployeePostion[key].ToString();
        hdnCurrentKey.Value = key.ToString();

        EnableDisableNavigation(key);
        LoadEmployeePosition();
        LoadGrid(true);
    }

    /// <summary>
    /// Show Hide Copy New
    /// </summary>
    private void ShowHideCopyNew()
    {
        if (hdnEmpPositionID.Value == "0")
        {
            btnCopyNewTop.Enabled = btnCopyNewBottom.Enabled = false;
            btnCopyNewTop.ImageUrl = btnCopyNewBottom.ImageUrl = IMG_COPY_NEW_DISABLE;
        }
        else
        {
            btnCopyNewTop.Enabled = btnCopyNewBottom.Enabled = true;
            btnCopyNewTop.ImageUrl = btnCopyNewBottom.ImageUrl = IMG_COPY_NEW;
        }
    }
    #endregion

    #endregion

    #region "Constants"

    #region "Message Constants"
    private const string MSG01 = "Employee position saved successfully";
    private const string MSG02 = "Failed to save employee position";
    private const string MSG03 = "Employee position you entered is already exists";
    private const string MSG04 = "Employee position copied successfully";
    private const string MSG05 = "Failed to copy position";
    private const string MSG06 = "Employee position is already exists";
    #endregion

    #region "Image Path Constants"
    private const string IMG_NEXT = "../Images/pegi_next.jpg";
    private const string IMG_NEXT_DISABLE = "../Images/pegi_next_dactivate.jpg";
    private const string IMG_PREVIOUS = "../Images/pegi_previous.jpg";
    private const string IMG_PREVIOUS_DISABLE = "../Images/pegi_previous_dactivate.jpg";
    private const string IMG_FIRST = "../Images/pop_txtfld_arrow4.jpg";
    private const string IMG_FIRST_DISABLE = "../Images/pegi_first_dactivate.jpg";
    private const string IMG_LAST = "../Images/pop_txtfld_arrow1.jpg";
    private const string IMG_LAST_DISABLE = "../Images/pegi_last_dactivate.jpg";
    private const string IMG_SAVE_NEW_DISABLE = "../Images/save_new_btn_dactivate.jpg";
    private const string IMG_SAVE_DISABLE = "../Images/save_deactive.jpg";
    private const string IMG_DELETE_USER_DISABLE = "~/Images/delete_icon_disable.jpg";
    private const string IMG_UNDO_DISABLE = "~/Images/undo_disable.jpg";
    private const string IMG_SAVE = "~/Images/save_btn.jpg";
    private const string IMG_SAVE_NEW = "~/Images/save_new_btn.jpg";
    private const string IMG_UNDO = "~/Images/undo_btn.jpg";
    private const string IMG_COPY_NEW = "~/Images/CopyNew_Active.png";
    private const string IMG_COPY_NEW_DISABLE = "~/Images/CopyNew_Deactive.png";
    #endregion

    #region "Page Names"
    private const string PAGE_EMPLOYEE_POSITIONS = "EmployeePosition.aspx";
    private const string PAGE_ADD_EDIT_PAGE_EMPLOYEE_POSITION = "AddEditEmployeePosition.aspx";
    private const string PAGE_PUBLISH_DATA = "PropagateDistributedData.aspx";
    #endregion

    #region "Session"
    private const string SESS_USER_ID = "sess_UserID";
    private const string SESS_POSITION_ID = "PositionID";
    private const string SESS_ACCESS_RIGHT = "sess_AccessRights";
    private const string SESS_IS_CALLED_FROM_PUBLISH = "IsCalledFromPublish";
    private const string SESS_MESSAGE = "message";
    #endregion

    #endregion
}