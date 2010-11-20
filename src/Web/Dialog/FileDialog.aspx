﻿<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="FileDialog.aspx.cs"
    Inherits="Cynthia.Web.Dialog.FileDialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <portal:StyleSheetCombiner ID="StyleSheetCombiner" runat="server" />
    
    <style type="text/css">
     #GalleryPreview{ table-layout: fixed; width: 450px; height: 400px; margin: 0 auto;float: left; border: 0px solid #0000ff; }
     #GalleryPreview_VerticalFix { width: 450px; height: 400px; display: table-cell; text-align: left; border: 0px solid #00ff00; }
     #GalleryPreview img { max-width: 550px; max-height: 550px;}
     .boldtext { font-weight: bold;}
     
    </style>
</head>
<body  class="filedialog">
    <form id="form1" runat="server">
    <portal:ScriptLoader ID="ScriptInclude" runat="server" IncludejQueryFileTree="true"
        IncludeOomph="false" />
    <div id="filewrapper" style="padding: 10px;">
    <table cellpadding="3" style="width: 100%">
        <tr>
            <th style="text-align: center; background-color: #E0DFE3; width: 50%;">
                <asp:Literal ID="litCreateFolder" runat="server" />
            </th>
            <th style="text-align: center; background-color: #E0DFE3; width: 50%;">
                <asp:Literal ID="litUpload" runat="server" />
            </th>
        </tr>
        <tr>
            <td style="background-color: #F0EFF1; width: 50%;" valign="top">
                <span style="font-size: x-small">
                <asp:Literal ID="litFolderInstructions" runat="server" /> </span>
                <asp:Panel ID="pnlUpload" runat="server" Visible="false" BackColor="#F0EFF1">
                    <asp:Panel ID="pnlNewFolder" runat="server" CssClass="settingrow" DefaultButton="btnNewFolder">
                        <asp:TextBox ID="txtNewDirectory" runat="server" Style="width: 150px" MaxLength="150"></asp:TextBox>
                        <asp:Button ID="btnNewFolder" runat="server" ValidationGroup="newfolder" />
                        <asp:RequiredFieldValidator ID="requireFolder" runat="server" ControlToValidate="txtNewDirectory"
                            Display="Dynamic" ValidationGroup="newfolder" />
                        <asp:RegularExpressionValidator ID="regexFolder" runat="server" ControlToValidate="txtNewDirectory"
                            Display="Dynamic" ValidationGroup="newfolder" />
                        <asp:Label ID="lblError" runat="server" CssClass="txterror" />
                    </asp:Panel>
                </asp:Panel>
            </td>
            <td style="background-color: #F0EFF1; width: 50%;" valign="top">
                <span style="font-size: x-small">
                <asp:Literal ID="litUploadInstructions" runat="server" /> </span>
                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnUpload" CssClass="settingrow">
                    <NeatUpload:InputFile ID="file" Style="width: 220px; height: 23px" runat="server" />
                    <NeatUpload:ProgressBar ID="progressBar" runat="server">
                        <cy:SiteLabel ID="SiteLabel2" runat="server" ConfigKey="CheckProgressText"> </cy:SiteLabel>
                    </NeatUpload:ProgressBar>
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" ValidationGroup="upload">
                    </asp:Button>
                    <asp:RegularExpressionValidator ID="regexFile" ControlToValidate="file" Display="Dynamic"
                        EnableClientScript="True" runat="server" ValidationGroup="upload" />
                    <asp:RequiredFieldValidator ID="reqFile" runat="server" ControlToValidate="file"
                        Display="Dynamic" ValidationGroup="upload" />
                    
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="background-color: #F0EFF1;" valign="top" align="right">
                <div style="font-size: x-small">
                        <asp:CheckBox ID="chkConstrainImageSize" runat="server" Checked="true" />&nbsp;&nbsp;
                        <cy:SiteLabel ID="lbl1" ForControl="txtMaxWidth" runat="server" ConfigKey="FileBrowserMaxWidth" EnableViewState="false" CssClass="boldtext"> </cy:SiteLabel>
                        <asp:TextBox ID="txtMaxWidth" runat="server" CssClass="smalltextbox" />
                        <cy:SiteLabel ID="SiteLabel3" ForControl="txtMaxWidth" runat="server" ConfigKey="FileBrowserMaxHeight" EnableViewState="false" CssClass="boldtext"> </cy:SiteLabel>
                        <asp:TextBox ID="txtMaxHeight" runat="server" CssClass="smalltextbox" />
                       
                    </div>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <tr>
            <td valign="top" style="height"1000px;">
                <ul class="rootfolder">
                    <li class="expanded">
                        <asp:HyperLink ID="lnkRoot" runat="server"></asp:HyperLink>
                        <asp:Panel ID="pnlFileTree" runat="server"  />
                    </li>
                </ul>
            </td>
            <td valign="top">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td valign="top" style="width: 100%">
                <asp:Literal ID="litHeading" Visible="false" runat="server" />
                <span style="font-size: small"><asp:Literal ID="litFileSelectInstructions" runat="server" /> </span>
                <div class="settingrow">
                    <asp:Button ID="btnSubmit" runat="server" Text="Select File" Width="90px" />
                    <asp:TextBox ID="txtSelection" runat="server" Style="width: 350px; border: 0px" Enabled="false" />&nbsp;
                </div>
                <div class="settingrow">
                    <cy:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="spacer" EnableViewState="false"> </cy:SiteLabel>
                </div>
                <div id="GalleryPreview">
                    <div id="GalleryPreview_VerticalFix">
                        <asp:Image ID="imgPreview" runat="server" ImageUrl="~/Data/SiteImages/1x1.gif" /><br />
                        <asp:HyperLink id="lnkImageCropper" runat="server" />
                        <asp:LinkButton ID="btnDelete" runat="server" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div class="clearpanel">
        <asp:HiddenField ID="hdnFolder" runat="server" />
        <asp:HiddenField ID="hdnFileUrl" runat="server"  />
    </div>
    </div>  
    <portal:CGoogleAnalyticsScript ID="CGoogleAnalyticsScript1" runat="server" />
    </form>
</body>
</html>
