<%@ Page Language="c#" CodeBehind="EditPost.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="Cynthia.Web.GroupUI.GroupPostEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <cy:CornerRounderTop ID="ctop1" runat="server" />
    <asp:Panel ID="pnlWrapper" runat="server" CssClass="panelwrapper groupmodule groupeditpost">
        <asp:Panel ID="pnlEdit" runat="server" CssClass="modulecontent" DefaultButton="btnUpdate">
            <h2 class="grouptopiccrumbs">
                <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink>
                &gt; <a href="" id="lnkGroup" runat="server"></a>&nbsp;&gt;
                <asp:Label ID="lblTopicDescription" runat="server"></asp:Label>
            </h2>
            <fieldset>
                <legend>
                    <asp:Literal ID="litGroupPostLabel" runat="server" />
                </legend>
                <div class="settingrow groupdesc">
                    <asp:Literal ID="litGroupDescription" runat="server" />
                </div>
                <div class="settingrow">
                    <cy:SiteLabel ID="lblSubjectLabel" runat="server" ForControl="txtSubject" CssClass="settinglabel"
                        ConfigKey="GroupPostEditSubjectLabel" ResourceFile="GroupResources"> </cy:SiteLabel>
                    <asp:TextBox ID="txtSubject" runat="server" MaxLength="255" CssClass="verywidetextbox forminput"></asp:TextBox>
                </div>
                <div>&nbsp;</div>
                <div class="settingrow">
                <cye:EditorControl ID="edMessage" runat="server">
                </cye:EditorControl>
                </div>
                <asp:Panel ID="pnlNotify" runat="server">
                <div  class="settingrow">
                    <cy:SiteLabel ID="lblNotifyOnReply" runat="server" ForControl="chkNotifyOnReply"
                        CssClass="settinglabel" ConfigKey="GroupPostEditNotifyLabel" ResourceFile="GroupResources"> </cy:SiteLabel>
                    <asp:CheckBox ID="chkNotifyOnReply" runat="server" CssClass="forminput"></asp:CheckBox>
                </div>
                <div  class="settingrow">
                    <cy:SiteLabel ID="SiteLabel1" runat="server" ForControl="chkSubscribeToGroup"
                        CssClass="settinglabel" ConfigKey="SubscribeToAllOfThisGroup" ResourceFile="GroupResources"> </cy:SiteLabel>
                    <asp:CheckBox ID="chkSubscribeToGroup" runat="server" CssClass="forminput"></asp:CheckBox>
                </div>
                </asp:Panel>
                <div class="settingrow">
                    <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
                    <asp:RequiredFieldValidator ID="reqSubject" runat="server" ControlToValidate="txtSubject" ValidationGroup="Group"
                        Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <asp:Panel ID="pnlAntiSpam" runat="server">
                    <cy:CaptchaControl ID="captcha" runat="server" />
                </asp:Panel>
                <div class="settingrow">
                    <cy:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                    <div class="forminput">
                    <portal:CButton ID="btnUpdate" runat="server" ValidationGroup="Group" />&nbsp;
                    <portal:CButton ID="btnDelete" runat="server" CausesValidation="false" />&nbsp;
                    <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;
                    
                    </div>
                </div>
                <div class="settingrow">&nbsp;</div>
                <asp:Repeater ID="rptMessages" runat="server" EnableViewState="False">
                    <ItemTemplate>
                        <div class="grouppostheader">
                            <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem), "PostDate", TimeOffset)%>
                        </div>
                        <div class="postwrapper">
                            <div class="postleft">
                                <div class="grouppostusername">
                                    <%# SiteUtils.GetProfileLink(DataBinder.Eval(Container.DataItem,"UserID"),DataBinder.Eval(Container.DataItem,"PostAuthor")) %>
                                </div>
                                <div class="grouppostuseravatar">
                                    <%# GetAvatarUrl(DataBinder.Eval(Container.DataItem,"PostAuthorAvatar").ToString()) %>
                                    <cy:Gravatar ID="grav1" runat="server" Email='<%# Eval("AuthorEmail") %>' MaxAllowedRating='<%# MaxAllowedGravatarRating %>'
                                        Visible='<%# allowGravatars %>' />
                                </div>
                                <div class="grouppostuserattribute">
                                    <cy:SiteLabel ID="lblTotalPosts" runat="server" ConfigKey="ManageUsersTotalPostsLabel" />
                                    <%# DataBinder.Eval(Container.DataItem,"PostAuthorTotalPosts") %>
                                </div>
                                <div class="grouppostuserattribute">
                                    <NeatHtml:UntrustedContent ID="UntrustedContent2" runat="server" TrustedImageUrlPattern='<%# Cynthia.Web.Framework.SecurityHelper.RegexRelativeImageUrlPatern %>'
                                        ClientScriptUrl="~/ClientScript/NeatHtml.js">
                                        <%# DataBinder.Eval(Container.DataItem, "PostAuthorSignature").ToString()%>
                                    </NeatHtml:UntrustedContent>
                                </div>
                                <div class="grouppostuserattribute">
                                    <portal:GroupUserTopicLink ID="lnkUserPosts" runat="server" UserId='<%# GetUserId(DataBinder.Eval(Container.DataItem,"UserID")) %>'
                                        TotalPosts='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"PostAuthorTotalPosts")) %>' />
                                </div>
                            </div>
                            <div class="postright">
                                <div class="posttopic">
                                    <h3>
                                        <%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "Subject").ToString())%></h3>
                                </div>
                                <div class="postbody">
                                    <br />
                                    <NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" TrustedImageUrlPattern='<%# allowedImageUrlRegexPattern %>'
                                        ClientScriptUrl="~/ClientScript/NeatHtml.js">
                                        <%# DataBinder.Eval(Container.DataItem, "Post").ToString()%>
                                    </NeatHtml:UntrustedContent>
                                </div>
                                <br />
                                <br />
                                <br />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </fieldset>
        </asp:Panel>
        <asp:HiddenField ID="hdnReturnUrl" runat="server" />
    </asp:Panel>
    <cy:CornerRounderBottom ID="cbottom1" runat="server" />
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
