<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ChangePassword.aspx.cs" Inherits="Cynthia.Web.UI.Pages.ChangePassword" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server" >
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel id="pnlPassword" runat="server" CssClass="panelwrapper login">
<div class="modulecontent">
<fieldset>
    <legend>
        <cy:SiteLabel id="lblRegisterLabel" runat="server" ConfigKey="ChangePasswordLabel" UseLabelTag="false"> </cy:SiteLabel>
    </legend>
    <asp:ChangePassword ID="ChangePassword1" runat="server" >
    <ChangePasswordTemplate>
        <div >
            <div class="settingrow">
                <strong><cy:SiteLabel id="lbloldpwd" runat="server" ForControl="CurrentPassword"  ConfigKey="ChangePasswordCurrentPasswordLabel"> </cy:SiteLabel></strong>
                <br /><asp:TextBox ID="CurrentPassword" runat="server" TextMode="password"  />
            </div>
            <div class="settingrow">
               <strong><cy:SiteLabel id="SiteLabel1" runat="server" ForControl="NewPassword" ConfigKey="ChangePasswordNewPasswordLabel"> </cy:SiteLabel></strong>
                <br /><asp:TextBox ID="NewPassword" runat="server" TextMode="password"  />
            </div>
            <div class="settingrow">
                <strong><cy:SiteLabel id="SiteLabel2" runat="server" ForControl="ConfirmNewPassword" ConfigKey="ChangePasswordConfirmNewPasswordLabel"> </cy:SiteLabel></strong>
                <br /><asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="password"  />
            </div>
            <div class="settingrow">
                <portal:CButton ID="ChangePasswordPushButton" CommandName="ChangePassword" Text="Change Password" runat="server" ValidationGroup="ChangePassword1"  />
                <portal:CButton ID="CancelPushButton" CommandName="Cancel" Text="Cancel" runat="server" CausesValidation="false"  />
            </div>
            <div class="settingrow">
                <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="ChangePassword1" />
                <asp:RequiredFieldValidator ControlToValidate="CurrentPassword" 
	                ID="CurrentPasswordRequired" 
	                Display="None"
	                runat="server" 
	                ValidationGroup="ChangePassword1"></asp:RequiredFieldValidator>
	            <asp:RequiredFieldValidator 
                  ControlToValidate="NewPassword" 
                  ID="NewPasswordRequired" 
                  runat="server" 
                  Display="None"
                  ValidationGroup="ChangePassword1"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator 
                  ControlToValidate="ConfirmNewPassword" 
                  ID="ConfirmNewPasswordRequired" 
                  runat="server" 
                  Display="None"
                  ValidationGroup="ChangePassword1"></asp:RequiredFieldValidator>
                <asp:CompareValidator 
                  ControlToCompare="NewPassword" 
                  ControlToValidate="ConfirmNewPassword"
                  ID="NewPasswordCompare" 
                  runat="server" 
                  Display="None"
                  ValidationGroup="ChangePassword1"></asp:CompareValidator>
                <asp:RegularExpressionValidator ID="NewPasswordRegex" runat="server" ControlToValidate="NewPassword" Display="None" ValidationGroup="ChangePassword1"></asp:RegularExpressionValidator>
                <asp:CustomValidator ID="NewPasswordRulesValidator" runat="server" ControlToValidate="NewPassword" Display="None" ValidationGroup="ChangePassword1"></asp:CustomValidator>
                <asp:Literal ID="FailureText" runat="server" EnableViewState="false" />
            </div>
        </div>
    </ChangePasswordTemplate>
    </asp:ChangePassword>
</fieldset>
</div>
</asp:Panel> 
<cy:CornerRounderBottom id="cbottom1" runat="server" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
