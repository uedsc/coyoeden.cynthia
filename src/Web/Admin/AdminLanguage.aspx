﻿<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminLanguage.aspx.cs" Inherits="Cynthia.Web.AdminUI.AdminLanguagePage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkCoreData" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
    </div>
    <portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlLanguage" runat="server" CssClass="art-Post-inner panelwrapper admin adminlanguage">
        <h2 class="moduletitle heading">
            <asp:Literal ID="litHeading" runat="server" /></h2>
            <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
        <div class="modulecontent ">
            <cy:CGridView ID="grdLanguage" runat="server" AllowPaging="false" AllowSorting="false"
                AutoGenerateColumns="false"  DataKeyNames="Guid" EnableTheming="false">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="buttonlink" Tezt='<%# Resources.Resource.LanguageGridEditButton %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnGridUpdate" runat="server" Text='<%# Resources.Resource.LanguageGridUpdateButton %>'
                                CommandName="Update" />
                            <asp:Button ID="btnGridDelete" runat="server" Text='<%# Resources.Resource.LanguageGridDeleteButton %>'
                                CommandName="Delete" />
                            <asp:Button ID="btnGridCancel" runat="server" Text='<%# Resources.Resource.LanguageGridCancelButton %>'
                                CommandName="Cancel" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Name") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtName" Columns="20" Text='<%# Eval("Name") %>' runat="server"
                                MaxLength="255" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Code") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCode" Columns="20" Text='<%# Eval("Code") %>' runat="server"
                                MaxLength="2" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Sort") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtSort" Columns="20" Text='<%# Eval("Sort") %>' runat="server"
                                MaxLength="4" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </cy:CGridView>
            <div class="settingrow">
                <portal:CButton ID="btnAddNew" runat="server" />
            </div>
            <div class="modulepager">
                <portal:CCutePager ID="pgrLanguage" runat="server" />
            </div>
        </div>
        </portal:CPanel>
        <div class="cleared"></div>
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
