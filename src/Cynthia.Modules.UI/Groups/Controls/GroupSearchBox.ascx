<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="GroupSearchBox.ascx.cs" Inherits="Cynthia.Web.GroupUI.GroupSearchBox" %>
<asp:Panel ID="pnlGroupSearch" runat="server" CssClass="settingrow groupsearch" DefaultButton="btnSearch">
<asp:TextBox ID="txtSearch" runat="server" CssClass="widetextbox groupsearchbox" />
<portal:CButton ID="btnSearch" runat="server" ValidationGroup="groupsearch" />
<asp:RequiredFieldValidator ID="reqSearchText" runat="server" ControlToValidate="txtSearch" Display="Dynamic" ValidationGroup="groupsearch" />
</asp:Panel>
