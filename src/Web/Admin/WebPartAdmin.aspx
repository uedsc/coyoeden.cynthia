<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="WebPartAdmin.aspx.cs" Inherits="Cynthia.Web.AdminUI.WebPartAdminPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlWrapper" runat="server" CssClass="panelwrapper admin webpartadmin">
<div class="modulecontent">
<fieldset>
    <legend>
       <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
       <asp:HyperLink ID="lnkAdvancedTools" runat="server" />&nbsp;&gt;
       <asp:HyperLink ID="lnkWebPartAdmin" runat="server" />
    </legend>
    
    <fieldset>
    <legend>
    <cy:SiteLabel id="Sitelabel4" runat="server" ConfigKey="WebPartAvailableNameLabel"> </cy:SiteLabel>
    </legend>
    <asp:Label ID="lblNoAvailableWebParts" runat="server" />
    <cy:CGridView ID="grdAvailableParts" runat="server"
     AllowPaging="false"
     AllowSorting="false"
     AutoGenerateColumns="false"
     EnableViewState="true"
     DataKeyNames="FullName" SkinID="plain">
     <Columns>
  
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="left" >
            <ItemTemplate>
                  <%# Eval("FullName")%>
            </ItemTemplate>
            <EditItemTemplate>
                
                <div class="settingrow">
                    <cy:SiteLabel id="Sitelabel4" runat="server" CssClass="settinglabel" ConfigKey="WebPartClassNameLabel"> </cy:SiteLabel>
                    <asp:Label ID="lblClassName" runat="server" />
                </div>
                <div class="settingrow">
                    <cy:SiteLabel id="Sitelabel5" runat="server" CssClass="settinglabel" ConfigKey="WebPartAssemblyNameLabel"> </cy:SiteLabel>
                    <asp:Label ID="lblAssemblyName" runat="server" />
                </div>
                <div class="settingrow">
                    <cy:SiteLabel id="Sitelabel6" runat="server" CssClass="settinglabel" ConfigKey="WebPartTitleLabel"> </cy:SiteLabel>
                    <asp:Label ID="lblTitle" runat="server" />
                </div>
                <div class="settingrow">
                    <cy:SiteLabel id="Sitelabel7" runat="server" CssClass="settinglabel" ConfigKey="WebPartDescriptionLabel"> </cy:SiteLabel>
                    <asp:Label ID="lblDescription" runat="server" />
                </div>
                <div class="settingrow">
                    <cy:SiteLabel id="Sitelabel1" runat="server" CssClass="settinglabel" ConfigKey="WebPartAvailableForMyPageLabel"> </cy:SiteLabel>
                    <asp:CheckBox ID="chkAvailableForMyPage" runat="server"></asp:CheckBox>
                </div>
                <div class="settingrow">
                    <cy:SiteLabel id="Sitelabel2" runat="server" CssClass="settinglabel" ConfigKey="WebPartAllowMultipleInstancesOnMyPageLabel"> </cy:SiteLabel>
                    <asp:CheckBox ID="chkAllowMultipleInstancesOnMyPage" runat="server"></asp:CheckBox>
                </div>
                <div class="settingrow">
                    <cy:SiteLabel id="Sitelabel3" runat="server" CssClass="settinglabel" ConfigKey="WebPartAvailableForContentSystemLabel"> </cy:SiteLabel>
                    <asp:CheckBox ID="chkAvailableForContentSystem" runat="server"></asp:CheckBox>
                </div>
                <div  class="settingrow">
		            <cy:SiteLabel id="lblIcon" runat="server" CssClass="settinglabel" ConfigKey="WebPartIconLabel" > </cy:SiteLabel>
		            <asp:DropDownList id="ddIcons" runat="server" DataValueField="Name" DataTextField="Name"></asp:DropDownList>
		            <img id="imgIcon" alt="" src=""  runat="server" />
		        </div>
		        
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                <asp:Button id="btnGridEdit" runat="server" Text='<%# Resources.Resource.WebPartAdminEditButton %>' ToolTip='<%# Resources.Resource.WebPartAdminEditButton %>' CommandArgument='<%# Eval("Assembly.FullName")%>' CommandName="edit" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:Button id="btnGridUpdate" runat="server" Text='<%# Resources.Resource.WebPartAdminUpdateButton %>' ToolTip='<%# Resources.Resource.WebPartAdminUpdateButton %>' CommandName="update" />
                <asp:Button id="btnGridCancel" runat="server" Text='<%# Resources.Resource.ContentManagerCancelButton %>' ToolTip='<%# Resources.Resource.ContentManagerCancelButton %>' CommandName="cancel" />
          
            </EditItemTemplate>
        
        </asp:TemplateField>
     </Columns>
 </cy:CGridView>
    </fieldset>
    
    
    <fieldset>
    <legend>
        <cy:SiteLabel id="Sitelabel8" runat="server" ConfigKey="WebPartInstalledWebPartsLabel"> </cy:SiteLabel>
        <portal:CHelpLink ID="CynHelpLink4" runat="server" HelpKey="webpartinstallationhelp" />	
    </legend>

<cy:CGridView ID="grdWebParts" runat="server"
     AllowPaging="false"
     AllowSorting="true"
     AutoGenerateColumns="false"
     EnableViewState="false" CellPadding="3"
     DataKeyNames="WebPartID"
     SkinID="plain">
     <Columns>
        <asp:TemplateField >
            <ItemTemplate>
                <img src='<%# Page.ResolveUrl("~/Data/SiteImages/FeatureIcons/" + DataBinder.Eval(Container.DataItem,"ImageUrl")) %>' 
                alt='<%# DataBinder.Eval(Container.DataItem,"Title")%>' />
                &nbsp;<asp:HyperLink id="editLink"  runat="server"
			            Text="<%# Resources.Resource.EditImageAltText %>" ToolTip="<%# Resources.Resource.EditImageAltText %>"  
			            ImageUrl='<%# Page.ResolveUrl("~/Data/SiteImages/" + EditContentImage) %>' 
			            NavigateUrl='<%# this.SiteRoot + "/Admin/WebPartEdit.aspx?part=" + DataBinder.Eval(Container.DataItem,"WebPartID")  %>' 
			             />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Title" ReadOnly="true" SortExpression="Title" />
        <asp:BoundField DataField="Description" ReadOnly="true" SortExpression="Description" />
        <asp:BoundField DataField="ClassName" ReadOnly="true" SortExpression="ClassName" />
       
        <asp:TemplateField ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                <img src='<%# Page.ResolveUrl("~/Data/SiteImages/" + DataBinder.Eval(Container.DataItem,"AvailableForMyPage").ToString().ToLower()) %>.gif' alt='<%# DataBinder.Eval(Container.DataItem,"AvailableForMyPage")%>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                <img src='<%# Page.ResolveUrl("~/Data/SiteImages/" + DataBinder.Eval(Container.DataItem,"AllowMultipleInstancesOnMyPage").ToString().ToLower()) %>.gif' alt='<%# DataBinder.Eval(Container.DataItem,"AllowMultipleInstancesOnMyPage")%>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                <img src='<%# Page.ResolveUrl("~/Data/SiteImages/" + DataBinder.Eval(Container.DataItem,"AvailableForContentSystem").ToString().ToLower()) %>.gif' alt='<%# DataBinder.Eval(Container.DataItem,"AvailableForContentSystem")%>' />
            </ItemTemplate>
        </asp:TemplateField>
       
     </Columns>
 </cy:CGridView>
<div class="modulepager" style="float:left; clear:left;">
    <span id="spnPager" runat="server"></span>
</div>

    </fieldset>


</fieldset>
</div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" />

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
