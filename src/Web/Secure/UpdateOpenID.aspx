<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="UpdateOpenID.aspx.cs" Inherits="Cynthia.Web.UI.UpdateOpenIdPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">


        <div id="divOpenID" runat="server" class="settingrow">
            <cy:SiteLabel id="SiteLabel4" runat="server" ForControl="OpenIdLogin1" CssClass="settinglabelFixedHeight" ConfigKey="ManageUsersOpenIDURILabel"> </cy:SiteLabel>
            <div>
            <portal:COpenIdLogin ValidationGroup="oid" UriValidatorEnabled="false" ID="OpenIdLogin1" runat="server" CssClass="openid_login" />
            <asp:Label ID="lblLoginFailed" runat="server" EnableViewState="False" Visible="False" />
            <asp:Label ID="lblLoginCanceled" runat="server" EnableViewState="False" Visible="False" />
            <portal:CHelpLink ID="CynHelpLink11" runat="server" HelpKey="useropenidhelp" />
            </div>
        </div>

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

