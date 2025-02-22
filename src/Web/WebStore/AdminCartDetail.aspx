﻿<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdminCartDetail.aspx.cs" Inherits="WebStore.UI.AdminCartDetailPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<cy:CornerRounderTop id="ctop1" runat="server" />
<asp:Panel ID="pnlStoreManager" runat="server" CssClass="panelwrapper webstore webstorecart">
 <div class="modulecontent"> 
         <div class="settingrow">
            <cy:SiteLabel ID="SiteLabel4" runat="server" CssClass="settinglabel" ConfigKey="IPAddressLabel"
                ResourceFile="WebStoreResources" />
            <asp:Label ID="lblIPAddress" runat="server" />
        </div>
         <div class="settingrow">
                <cy:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="SiteUserLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblSiteUser" runat="server" />
                <asp:HyperLink ID="lnkUser" runat="server" />
            </div>
        <div class="settingrow">
            <cy:SiteLabel ID="LabelCustomerName" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerFirstNameLabel"
                ResourceFile="WebStoreResources" />
            <asp:Label ID="lblCustomerFirstName" runat="server" />
        </div>
        <div class="settingrow">
            <cy:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerLastNameLabel"
                ResourceFile="WebStoreResources" />
            <asp:Label ID="lblCustomerLastName" runat="server" />
        </div>
        <div class="settingrow">
            <cy:SiteLabel ID="LabelCustomerCompany" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerCompanyLabel"
                ResourceFile="WebStoreResources" />
            <asp:Label ID="lblCustomerCompany" runat="server" />
        </div>
        <div class="settingrow">
            <cy:SiteLabel ID="LabelCustomerAddressLine1" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerAddressLine1Label"
                ResourceFile="WebStoreResources" />
            <asp:Label ID="lblCustomerAddressLine1" runat="server" />
        </div>
        <div class="settingrow">
            <cy:SiteLabel ID="LabelCustomerAddressLine2" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerAddressLine2Label"
                ResourceFile="WebStoreResources" />
            <asp:Label ID="lblCustomerAddressLine2" runat="server" />
        </div>
        <div class="settingrow">
            <cy:SiteLabel ID="LabelCustomerCountry" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerCountryLabel"
                ResourceFile="WebStoreResources" />      
            <asp:Label ID="lblCustomerCountry" runat="server" />
        </div>
        <div class="settingrow" id="divCustomerState" runat="server">
            <cy:SiteLabel ID="LabelCustomerState" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerStateLabel"
                ResourceFile="WebStoreResources" />
            <asp:Label ID="lblCustomerGeoZone" runat="server" />
        </div>
        <div class="settingrow">
            <cy:SiteLabel ID="LabellCustomerSuburb" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerSuburbLabel"
                ResourceFile="WebStoreResources" />
            <asp:Label ID="lblCustomerSuburb" runat="server" />
        </div>
        <div class="settingrow">
            <cy:SiteLabel ID="LabelCustomerCity" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerCityLabel"
                ResourceFile="WebStoreResources" />
            <asp:Label ID="lblCustomerCity" runat="server" />
        </div>
        <div class="settingrow">
            <cy:SiteLabel ID="LabelCustomerPostalCode" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerPostalCodeLabel"
                ResourceFile="WebStoreResources" />
            <asp:Label ID="lblCustomerPostalCode" runat="server" />
        </div>
        <div class="settingrow">
            <cy:SiteLabel ID="LabelCustomerTelephoneDay" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerTelephoneDayLabel"
                ResourceFile="WebStoreResources" />
            <asp:Label ID="lblCustomerTelephoneDay" runat="server" />
        </div>
        <div class="settingrow">
            <cy:SiteLabel ID="LabelCustomerTelephoneNight" runat="server" CssClass="settinglabel"
                ConfigKey="CartOrderInfoCustomerTelephoneNightLabel" ResourceFile="WebStoreResources" />
            
            <asp:Label ID="lblCustomerTelephoneNight" runat="server" />
        </div>
        <div class="settingrow">
            <cy:SiteLabel ID="LabelCustomerEmail" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerEmailLabel" ResourceFile="WebStoreResources" />
            <asp:Label ID="lblCustomerEmail" runat="server" />
        </div>
        <div class="storerow">
            <asp:Repeater ID="rptCartItems" runat="server">
                <ItemTemplate>
                    <div>
                        <asp:Button ID="lnkDeleteItem" runat="server" CssClass="buttonlink" CommandArgument='<%# Eval("ItemGuid") %>'
                            CommandName="delete" Text="x" />
                        <%# Eval("Name") %>
                        <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("OfferPrice"))) %>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        
        </div>
        <asp:Panel ID="pnlSubTotal" runat="server" CssClass="settingrow">
            <cy:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="CartSubTotalLabel"
                ResourceFile="WebStoreResources" />
            <asp:Literal ID="litSubTotal" runat="server" />
        </asp:Panel>
        <div class="settingrow">
            <cy:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="spacer"  />
            <portal:CButton ID="btnDelete" runat="server" />
        </div>
        <asp:Panel ID="pnlCheckoutLog" runat="server" CssClass="clearpanel"></asp:Panel>
    
</div>
</asp:Panel>

<cy:CornerRounderBottom id="cbottom1" runat="server" />

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
