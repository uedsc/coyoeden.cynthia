<%@ Page Language="c#" ValidateRequest="false" CodeBehind="EditGroup.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="Cynthia.Web.GroupUI.GroupEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate" CssClass="panelwrapper groupedit">
        <div class="modulecontent">
            <fieldset>
                <legend>
                    <cy:SiteLabel ID="lblGroupLabel" runat="server" ConfigKey="GroupEditGroupLabel" ResourceFile="GroupResources" UseLabelTag="false">
                    </cy:SiteLabel>
                </legend>
                <div class="group">
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblCreatedDateLabel" runat="server" CssClass="settinglabel" ConfigKey="GroupEditCreatedDateLabel" ResourceFile="GroupResources">
                        </cy:SiteLabel>
                        <asp:Label ID="lblCreatedDate" runat="server" CssClass="Normal forminput"></asp:Label>
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblTitleLabel" runat="server" ForControl="txtTitle" CssClass="settinglabel"
                            ConfigKey="GroupEditTitleLabel" ResourceFile="GroupResources"> </cy:SiteLabel>
                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="100" CssClass="widetextbox forminput"></asp:TextBox>
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblDescriptionLabel" runat="server" ForControl="fckDescription"
                            CssClass="settinglabel" ConfigKey="GroupEditDescriptionLabel" ResourceFile="GroupResources" />
                    </div>
                    <div class="settingrow">
                    <cye:EditorControl ID="edContent" runat="server">
                        </cye:EditorControl>
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblAllowAnonymousPosts" runat="server" ForControl="chkAllowAnonymousPosts"
                            CssClass="settinglabel" ConfigKey="AllowAnonymousPostsLabel" ResourceFile="GroupResources"> </cy:SiteLabel>
                        <asp:CheckBox ID="chkAllowAnonymousPosts" runat="server" CssClass="forminput"></asp:CheckBox>
                    </div>
                    <div class="settingrow" id="divIsModerated" runat="server">
                        <cy:SiteLabel ID="lblIsModeratedLabel" runat="server" ForControl="chkIsModerated"
                            CssClass="settinglabel" ConfigKey="GroupEditIsModeratedLabel" ResourceFile="GroupResources"> </cy:SiteLabel>
                        <asp:CheckBox ID="chkIsModerated" runat="server" CssClass="forminput"></asp:CheckBox>
                    </div>
                    <div class="settingrow" id="divIsActive" runat="server">
                        <cy:SiteLabel ID="lblIsActiveLabel" runat="server" ForControl="chkIsActive" CssClass="settinglabel"
                            ConfigKey="GroupEditIsActiveLabel" ResourceFile="GroupResources" />
                        <asp:CheckBox ID="chkIsActive" runat="server" CssClass="forminput"></asp:CheckBox>
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblSortOrderLabel" runat="server" ForControl="txtSortOrder" CssClass="settinglabel"
                            ConfigKey="GroupEditSortOrderLabel" ResourceFile="GroupResources" />
                        <asp:TextBox ID="txtSortOrder" runat="server" MaxLength="5" CssClass="smalltextbox forminput"></asp:TextBox>
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblPostsPerPageLabel" runat="server" ForControl="txtPostsPerPage"
                            CssClass="settinglabel" ConfigKey="GroupEditPostsPerPageLabel" ResourceFile="GroupResources" />
                        <asp:TextBox ID="txtPostsPerPage" runat="server" MaxLength="5" CssClass="smalltextbox forminput"></asp:TextBox>
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="lblTopicsPerPageLabel" runat="server" ForControl="txtTopicsPerPage"
                            CssClass="settinglabel" ConfigKey="GroupEditTopicsPerPageLabel" ResourceFile="GroupResources" />
                        <asp:TextBox ID="txtTopicsPerPage" runat="server" MaxLength="5" CssClass="smalltextbox forminput"></asp:TextBox>
                    </div>
                    <div class="settingrow">
                        <asp:Label ID="lblError" runat="server" CssClass="txterror"></asp:Label>
                    </div>
                    <div class="settingrow">
                        <cy:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                        <div class="forminput">
                        <portal:CButton ID="btnUpdate" runat="server" />&nbsp;
                        <portal:CButton ID="btnDelete" runat="server" CausesValidation="false" />&nbsp;
                        <asp:HyperLink ID="lnkCancel" runat="server" />
                        <portal:CHelpLink ID="CynHelpLink1" runat="server" HelpKey="groupedithelp" />
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        <asp:HiddenField ID="hdnReturnUrl" runat="server" />
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
