<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTemplate/PStAdminMaster.master" AutoEventWireup="true" CodeFile="AddEditEmployeePosition.aspx.cs" Inherits="Admin_AddEditEmployeePosition" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PStPlaceHolder" runat="Server">

    <script src="../js/Common/ButtonActions.js" type="text/javascript"></script>
    <script src="../js/ValidateEmployeePosition.js" type="text/javascript"></script>
    <script src="../js/AddEditEmployeePosition.js" type="text/javascript"></script>

    <style>
        .gr_tick_outer {
            float: left;
            width: 9px;
            height: 9px;
            margin: 0 5px 0 0;
        }
    </style>

    <section class="body_contentouter">
        <section class="bluebox_outer">
            <section class="bluebox_top">
                <section class="bluebox_top_lt">
                </section>
                <section class="bluebox_top_rt">
                </section>
            </section>
            <section class="bluebox_midd">
                <section class="whitebox_outer2">
                    <section class="whitebox_top">
                        <section class="whitebox_top_lt">
                        </section>
                        <section class="whitebox_top_rt">
                        </section>
                    </section>
                    <section class="whitebox_midd">
                        <section class="whitebox_midd_content_comman">
                            <section class="heading_outer" style="height:30px;">
                                <hgroup>
                                    <h1 style="width:13%;">
                                        <asp:Label ID="lblHeader" Text="Add Edit Employee Position" runat="server" />
                                    </h1>
                                    <section ID="secProgress" style="cursor: wait; display:none;">
                                        <img src="../Images/loader1.gif" style="vertical-align: middle;" alt="Processing" />
                                    </section>
                                <section class="required" style="width:20%; float:right;">(<span>*</span>) required</section>
                                </hgroup>
                                <section class="clear"></section>
                            </section>
                            <section class="comman_innercontent_outer">
                                 <section class="btn_outer" style="margin-bottom: 10px;">
                                        <section class="btn_outer_lt">
                                            <asp:ImageButton ID="btnSaveNewTop" ImageUrl="../Images/save_new_btn.jpg" AlternateText="Save & New" 
                                                ToolTip="Save & New" runat="server" Width="111" Height="28" OnClick="btnSaveNew_Click" />
                                            <asp:ImageButton ID="btnSaveTop" ImageUrl="../Images/save_btn.jpg" AlternateText="Save" 
                                                ToolTip="Save" runat="server" Width="83" Height="28"  OnClick="btnSave_Click" />
                                            <asp:ImageButton ID="btnUndoTop" ImageUrl="../Images/undo_btn.jpg" AlternateText="Undo" 
                                                ToolTip="Undo" runat="server" Width="83" Height="28" OnClick="btnUndo_Click" OnClientClick="return Undo()" />
                                            <asp:ImageButton ID="btnCopyNewTop" ImageUrl="../Images/CopyNew_Active.png" AlternateText="Copy" 
                                                ToolTip="Copy/New" runat="server" Height="28" OnClick="btnCopyNew_Click" OnClientClick="ShowHideCurriculumLoader(true);"/>
                                            <asp:ImageButton ID="btnCloseTop" ImageUrl="../Images/close_btn.jpg" AlternateText="Close" 
                                                ToolTip="Close" runat="server" Width="83" Height="28" OnClick="btnClose_Click" OnClientClick="return IsFormModified();" />
                                        </section>
                                        <section class="btn_outer_rt">
                                             <section class="pegination">
                                                <ul>
                                                    <li>
                                                        <asp:ImageButton ID="btnFirstTop" ImageUrl="~/Images/pop_txtfld_arrow4.jpg" runat="server" AlternateText="<<" OnClientClick="return IsFormModified()" OnClick="btnFirst_Click" ToolTip="First" />
                                                    </li>
                                                    <li>&nbsp;</li>
                                                    <li>
                                                        <asp:ImageButton ID="btnPreviousTop" ImageUrl="../Images/pegi_previous.jpg" runat="server" AlternateText="<" OnClientClick="return IsFormModified()" OnClick="btnPrevious_Click" ToolTip="Previous" />
                                                    </li>
                                                    <li class="mid">
                                                        <asp:Label ID="lblRecordCountTop" Text="0 of 0" runat="server" />
                                                    </li>
                                                    <li>
                                                        <asp:ImageButton ID="btnNextTop" ImageUrl="../Images/pegi_next.jpg" runat="server" AlternateText=">" OnClientClick="return IsFormModified()" OnClick="btnNext_Click" ToolTip="Next" />
                                                    </li>
                                                    <li>&nbsp;</li>
                                                    <li>
                                                        <asp:ImageButton ID="btnLastTop" ImageUrl="~/Images/pop_txtfld_arrow1.jpg" runat="server" AlternateText=">>" OnClientClick="return IsFormModified()" OnClick="btnLast_Click" ToolTip="Last" />
                                                    </li>
                                               </ul>
                                                <section class="clear">
                                                </section>
                                            </section>
                                        </section>
                                        <section class="clear">
                                        </section>
                                        <asp:Label ID="lblMessage" Text="" runat="server" ViewStateMode="Disabled" />
                                    </section>
                                <section class="bluebox_outer">
                                    <section class="bluebox_top">
                                        <section class="bluebox_top_lt">
                                        </section>
                                        <section class="bluebox_top_rt">
                                        </section>
                                    </section>
                                    <section class="bluebox_midd">
                                        <section style="width:90%;">
                                        <section class="form_ltcommanUser" style="width: 42%!important;">
                                            <section>
                                                <section style="float:left; width:100%">                                       
                                                     <section class="changepassoword_row1" style="margin-top: 10px;">
                                                        <label class="lable_heading5" style="width: 30%!important;"><span>*</span>Position:</label>
                                                        <section class="txtfild_outer5" style="width:45%;">
                                                            <section class="txtfild_inner">
                                                                <asp:TextBox ID="txtPosition" autocomplete="off" MaxLength="100" runat="server" TabIndex="1"></asp:TextBox>
                                                            </section>
                                                        </section>
                                                        <section class="clear"></section>
                                                    </section>
                                                    <section class="changepassoword_row1" style="margin-top: 20px;">
                                                        <label class="lable_heading5" style="width: 30%!important;"><span>*</span>Description:</label>
                                                        <section class="txtfild_outer5" style="width:45%;">
                                                            <section class="txtfild_inner">
                                                                <asp:TextBox ID="txtDescription" autocomplete="off" MaxLength="200" runat="server" TabIndex="2"></asp:TextBox>
                                                            </section>
                                                        </section>
                                                        <section class="clear"></section>
                                                    </section>
                                                    <section class="changepassoword_row1" style="margin-top: 20px;">
                                                        <label class="lable_heading5" style="width: 30%!important;"><span>*</span>EVS Tech Setting:</label>
                                                        <section class="txtfild_outer5" style="width:32%;">
                                                            <section class="txtfild_inner">
                                                                 <asp:DropDownList ID="ddlHouseKeeping" TabIndex="3" runat="server" onblur="PopUp(this,'blur'); " onfocus="PopDown(this);" onmousedown="PopUp(this,'mousedown');" onchange="PopDown(this);">
                                                                    </asp:DropDownList>
                                                            </section>
                                                        </section>
                                                        <section class="clear"></section>
                                                    </section>
                                                    <section class="changepassoword_row1" style="margin-top: 20px;">
                                                        <label class="lable_heading5" style="width: 30%!important;"><span>*</span>Service:</label>
                                                        <section class="txtfild_outer5" style="width:32%;">
                                                            <section class="txtfild_inner">
                                                                 <asp:DropDownList ID="ddlService" TabIndex="4" runat="server" onblur="PopUp(this,'blur'); " onfocus="PopDown(this);" onmousedown="PopUp(this,'mousedown');" onchange="PopDown(this);">
                                                                    <asp:ListItem Value="0" Text=""></asp:ListItem>
                                                                    <asp:ListItem Value="1">EVS</asp:ListItem>
                                                                    <asp:ListItem Value="2">Food</asp:ListItem>
                                                                    <asp:ListItem Value="3">Linen/Laundry</asp:ListItem>
                                                                    <asp:ListItem Value="4">Transport</asp:ListItem>
                                                                 </asp:DropDownList>
                                                            </section>
                                                        </section>
                                                        <section class="clear"></section>
                                                    </section>
                                                    <section class="changepassoword_row1" style="margin-top: 20px;">
                                                        <label class="lable_heading5" style="width: 30%!important;"><span>*</span>Classification:</label>
                                                        <section class="txtfild_outer5" style="width:32%;">
                                                            <section class="txtfild_inner">
                                                                 <asp:DropDownList ID="ddlClassification" TabIndex="5" runat="server" onblur="PopUp(this,'blur'); " onfocus="PopDown(this);" onmousedown="PopUp(this,'mousedown');" onchange="PopDown(this);">
                                                                    <asp:ListItem Value="0" Text=""></asp:ListItem>
                                                                    <asp:ListItem Value="1">Employee</asp:ListItem>
                                                                    <asp:ListItem Value="2">Onsite Management</asp:ListItem>
                                                                    <asp:ListItem Value="3">Support</asp:ListItem>
                                                                    <asp:ListItem Value="4">DM</asp:ListItem>
                                                                    <asp:ListItem Value="5">RVP</asp:ListItem>
                                                                    <asp:ListItem Value="6">D.CEO</asp:ListItem>
                                                                    <asp:ListItem Value="7">CEO</asp:ListItem>
                                                                 </asp:DropDownList>
                                                            </section>
                                                        </section>
                                                        <section class="clear"></section>
                                                    </section>
                                                    <section class="changepassoword_row1" style="margin-top: 20px;">
                                                        <section class="form_ltcomman" style="width:50%">
                                                            <section id="secMgtTraining">
                                                                <label class="lable_heading5" style="width: 60%!important; margin-top:-4px;">Management Training:</label> 
                                                                <asp:CheckBox ID="chkMgtTraining" runat="server"></asp:CheckBox> 
                                                            </section>
                                                        </section>
                                                        <section class="form_ltcomman" style="width:50%">
                                                            <section id="secAnnualCertification" style="display: none;">
                                                                <label class="lable_heading5" style="width: 50%!important; margin-top:-4px;">Protecta Certification:</label> 
                                                                <asp:CheckBox ID="chkAnnualCertification" runat="server"></asp:CheckBox> 
                                                            </section>
                                                        </section>
                                                        <section class="clear"></section>
                                                        </section>
                                                </section>
                                                <section class="clear"></section>
                                            </section>
                                        </section>
                                        <section class="form_rtcommanUser" style="width: 40%!important; padding:5px 0 10px 0 ; float:left;">
                                            <section class="grid_outer" style="margin-bottom: 0px !important;">                                
                                                <telerik:RadAjaxPanel ID="rapCurriculum" runat="server" EnableAJAX="true" style="padding-top:5px;" 
                                                    ClientEvents-OnResponseEnd ="OnAJAXResponseEnd();" ClientEvents-OnRequestStart="ShowHideCurriculumLoader(true);">
                                                    <telerik:RadGrid ID="grdCurriculum" runat="server" AllowPaging="false" AllowSorting="false" AutoGenerateColumns="False" CellSpacing="0" 
                                                        GridLines="None" Skin="Windows7" SortingSettings-EnableSkinSortStyles="false" Height="260px"
                                                        OnNeedDataSource="grdCurriculum_NeedDataSource" OnItemDataBound="grdCurriculum_ItemDataBound" 
                                                        OnDetailTableDataBind="grdCurriculum_DetailTableDataBind" OnPreRender="grdCurriculum_PreRender">
                                                        <MasterTableView Name="Parent" AutoGenerateColumns="false" DataKeyNames="tcc_key" ClientDataKeyNames="tcc_key" Width="100%" HierarchyLoadMode="ServerBind">                                           
                                                            <Columns>                                
                                                                <telerik:GridBoundColumn HeaderText="tcc_key" UniqueName="tcc_key" DataField="tcc_key" Visible="false">
                                                                </telerik:GridBoundColumn>                                                   
                                                                <telerik:GridBoundColumn HeaderText="Curriculum" UniqueName="tcc_description" DataField="tcc_description" 
                                                                    HeaderStyle-Width="350px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" 
                                                                    SortExpression="tcc_description" ItemStyle-VerticalAlign="Middle" AllowSorting="true">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Select" UniqueName="chkIsSelected" HeaderStyle-Wrap="false"
                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ReadOnly="false">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkHeader" runat="server" onclick="checkAllRows(this);"/>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsSelected" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsSelected")) %>' />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>                                       
                                                            </Columns>
                                                            <DetailTables>
                                                                <telerik:GridTableView Name="child" DataKeyNames="tr_key, cctl_cc_key" AllowPaging="false" HierarchyLoadMode="ServerBind">
                                                                    <Columns>                                                        
                                                                        <telerik:GridBoundColumn HeaderText="Class" UniqueName="tr_name" DataField="tr_name" HeaderStyle-Width="350px">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn HeaderText="Version" UniqueName="tr_version" DataField="tr_version" HeaderStyle-HorizontalAlign="Right" 
                                                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" DataFormatString="{0:### ##0.00}">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="30">
                                                                            <ItemTemplate>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </telerik:GridTableView>
                                                            </DetailTables>
                                                        </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="false">    
                                                        <Scrolling AllowScroll="True" UseStaticHeaders="true" SaveScrollPosition="True" ScrollHeight="173px"></Scrolling>  
                                                    </ClientSettings>
                                                    <FilterMenu EnableImageSprites="False">
                                                    </FilterMenu>
                                                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Windows7">
                                                    </HeaderContextMenu>
                                                    </telerik:RadGrid>
                                                </telerik:RadAjaxPanel>
                                            </section>
                                        </section>
                                        </section>
                                        <section class="clear"></section>                                         
                                    </section>
                                    <section class="clear"></section>
                                </section>
                                <section class="bluebox_bott">
                                    <section class="bluebox_bott_lt">
                                    </section>
                                    <section class="bluebox_bott_rt">
                                    </section>
                                </section>
                                <section class="clear"></section>
                            </section>
                            <section class="btn_outer">
                                <section class="btn_outer_lt">
                                    <asp:ImageButton ID="btnSaveNewBottom" ImageUrl="../Images/save_new_btn.jpg" AlternateText="Save & New" 
                                        ToolTip="Save & New" runat="server" Width="111" Height="28" OnClick="btnSaveNew_Click" />
                                    <asp:ImageButton ID="btnSaveBottom" ImageUrl="../Images/save_btn.jpg" AlternateText="Save" 
                                        ToolTip="Save" runat="server" Width="83" Height="28"  OnClick="btnSave_Click" />
                                    <asp:ImageButton ID="btnUndoBottom" ImageUrl="../Images/undo_btn.jpg" AlternateText="Undo" 
                                        ToolTip="Undo" runat="server" Width="83" Height="28" OnClick="btnUndo_Click" OnClientClick="return Undo()" />
                                    <asp:ImageButton ID="btnCopyNewBottom" ImageUrl="../Images/CopyNew_Active.png" AlternateText="Copy" 
                                            ToolTip="Copy/New" runat="server" Height="28" OnClick="btnCopyNew_Click" OnClientClick="ShowHideCurriculumLoader(true)"/>
                                    <asp:ImageButton ID="btnCloseBottom" ImageUrl="../Images/close_btn.jpg" AlternateText="Close" 
                                        ToolTip="Close" runat="server" Width="83" Height="28" OnClick="btnClose_Click" OnClientClick="return IsFormModified()" />      
                                </section>
                                <section class="btn_outer_rt">
                                     <section class="pegination" style="display:none;">
                                        <ul>
                                            <li>
                                                <asp:ImageButton ID="btnFirstBottom" ImageUrl="~/Images/pop_txtfld_arrow4.jpg" runat="server" AlternateText="<<" OnClientClick="return IsFormModified()" OnClick="btnFirst_Click" ToolTip="First" />
                                            </li>
                                            <li>&nbsp;</li>
                                            <li>
                                                <asp:ImageButton ID="btnPreviousBottom" ImageUrl="../Images/pegi_previous.jpg" runat="server" AlternateText="<" OnClientClick="return IsFormModified()" OnClick="btnPrevious_Click" ToolTip="Previous" />
                                            </li>
                                            <li class="mid">
                                                <asp:Label ID="lblRecordCountBottom" Text="0 of 0" runat="server" /></li>
                                            <li>
                                                <asp:ImageButton ID="btnNextBottom" ImageUrl="../Images/pegi_next.jpg" runat="server" AlternateText=">" OnClientClick="return IsFormModified()" OnClick="btnNext_Click" ToolTip="Next" /></li>
                                            <li>&nbsp;</li>
                                            <li>
                                                <asp:ImageButton ID="btnLastBottom" ImageUrl="~/Images/pop_txtfld_arrow1.jpg" runat="server" AlternateText=">>" OnClientClick="return IsFormModified()" OnClick="btnLast_Click" ToolTip="Last" />
                                            </li>
                                        </ul>
                                        <section class="clear">
                                        </section>
                                     </section>
                                </section>
                                <section class="clear"></section>
                            </section>
                        </section>
                        <section class="clear"></section>
                    </section>
                </section>
                <section class="whitebox_bott">
                    <section class="whitebox_bott_lt">
                    </section>
                    <section class="whitebox_bott_rt">
                    </section>
                </section>
            </section>
            <section class="clear"></section>
        </section>
        <section class="bluebox_bott">
            <section class="bluebox_bott_lt">
            </section>
            <section class="bluebox_bott_rt">
            </section>
        </section>
        <section class="clear"></section>
    </section>

    <section id="popupConfirm" class="popupbx" style="width: 400px;">
        <section class="popupbx_top">
            <section class="popupbx_toplt">
            </section>
            <section class="popupbx_toprt">
            </section>
        </section>
        <section class="popupbx_mid">
            <section class="popupbx_mid_header">
                <section class="pop_close">
                    <asp:ImageButton ID="btnPopupClose" ImageUrl="../Images/popclose_bt.jpg" AlternateText="Close" ToolTip="Close" runat="server" OnClientClick="DisablePopup('popupConfirm'); return false;" />
                </section>
                <h1> Confirm</h1>
            </section>
            <section class="popupbx_mid_cont">
                <label id="lblAlert"> Do you want to save your data?</label>
            </section>
            <section class="popupbx_mid_cont_tab">
                <asp:ImageButton ID="btnPopupYes" ImageUrl="~/Images/yes_btn.jpg" AlternateText="Yes" ToolTip="Yes" Width="59" Height="28" border="0" runat="server" OnClientClick="return false;" />
                <asp:ImageButton ID="btnPopupNo" ImageUrl="~/Images/no_btn.jpg" AlternateText="No" ToolTip="No" Width="59" Height="28" border="0" runat="server" OnClientClick="return false;" />
            </section>
        </section>
        <section class="popupbx_bot">
            <section class="popupbx_botlt">
            </section>
            <section class="popupbx_botrt">
            </section>
        </section>
    </section>

    <asp:HiddenField ID="hdnUserID" Value="0" runat="server" />
    <asp:HiddenField ID="hdnLastEmpPositionID" Value="0" runat="server" />
    <asp:HiddenField ID="hdnEmpPositionID" Value="0" runat="server" />
    <asp:HiddenField ID="hdnFormModified" Value="0" runat="server" />
    <asp:HiddenField ID="hdnCurrentKey" Value="0" runat="server" />
    <asp:HiddenField ID="hdnLastButtonClicked" Value="" runat="server" />
    <asp:HiddenField ID="hdnHeaderClientID" runat="server" Value="" />
    <asp:HiddenField ID="hdnHierarchyState" Value="" runat="server" />
    <asp:HiddenField ID="hdnIsPublished" Value="0" runat="server" />
    <asp:HiddenField ID="hdnPublishSession" runat="server" />

</asp:Content>

