<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="FlashMovieModule.ascx.cs" Inherits="Cynthia.Web.FlashUI.FlashMovieModule" %>

<portal:ModuleTitleControl EditText="Edit" EditUrl="~/FlashModule/FlashMovieEdit.aspx" runat="server" id="TitleControl" Visible="false" />

<asp:Panel ID="pnlFlashMovie" runat="server" cssclass="panelwrapper flashmodule">

<cy:FlashMovie id="flash1" runat="server" 
 Movie="~/Data/Sites/1/flash/flipbook.swf"
 Width="600" 
 Height="200" 
 FlashVersion="6">
 <cy:FlashParameter runat="server" Name="menu" Value="false" />
 <cy:FlashVariable runat="server" Name="approot" 
     Value='<%# Request.ApplicationPath %>' />
 </cy:FlashMovie>


</asp:Panel>