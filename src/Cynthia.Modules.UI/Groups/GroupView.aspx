<%@ Page language="c#" Codebehind="GroupView.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="Cynthia.Web.GroupUI.GroupView" %>
<%@ Register TagPrefix="group" TagName="SearchBox" Src="~/Groups/Controls/GroupSearchBox.ascx" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink> 
</div>
<portal:CPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<cy:CornerRounderTop id="ctop1" runat="server" EnableViewState="false" />
<asp:Panel id="pnlGroup" runat="server" cssclass="art-Post-inner panelwrapper groupview" EnableViewState="false">
     <h2 class="moduletitle"><asp:Literal ID="litGroupTitle" runat="server" /></h2>
     <portal:CPanel ID="CynPanel1" runat="server" ArtisteerCssClass="art-PostContent">
     <div class="modulecontent">
    <div class="settingrow groupdesc">
        <asp:Literal ID="litGroupDescription" runat="server" />
    </div>
    <group:SearchBox id="sb1" runat="server" />
    <asp:Panel ID="pnlNotify" runat="server" Visible="false" CssClass="groupnotify">
        <asp:HyperLink ID="lnkNotify" runat="server" ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/FeatureIcons/email.png"  %>' NavigateUrl='<%# notificationUrl %>' 
				 Text='<%# Resources.GroupResources.SubscribeLink %>' />
                 &nbsp;<asp:HyperLink ID="lnkNotify2" runat="server" NavigateUrl='<%# notificationUrl %>' 
				 Text='<%# Resources.GroupResources.SubscribeLongLink %>'
                 ToolTip='<%# Resources.GroupResources.SubscribeLongLink %>' />
                
    </asp:Panel>
    <div class="modulepager">
        <portal:CCutePager ID="pgrTop" runat="server" />
        <a href="" class="ModulePager" id="lnkNewTopic" runat="server"></a>
        <asp:HyperLink ID="lnkLogin" runat="server" CssClass="ModulePager" />
    </div>
	<table summary='<%# Resources.GroupResources.GroupViewTableSummary %>' border="0" cellspacing="1" width="100%" cellpadding="3">
		<thead><tr class="moduletitle">
		    <th id='<%# Resources.GroupResources.GroupViewSubjectLabel %>'>
				<cy:SiteLabel id="SiteLabel1" runat="server" ConfigKey="GroupViewSubjectLabel" ResourceFile="GroupResources" UseLabelTag="false" />
			</th>
			<th id='<%# Resources.GroupResources.GroupViewStartedByLabel %>'>
				<cy:SiteLabel id="lblGroupStartedBy" runat="server" ConfigKey="GroupViewStartedByLabel" ResourceFile="GroupResources" UseLabelTag="false" />
			</th>
			<th id='<%# Resources.GroupResources.GroupViewViewCountLabel %>'>
				<cy:SiteLabel id="lblTotalViewsCountLabel" runat="server" ConfigKey="GroupViewViewCountLabel" ResourceFile="GroupResources" UseLabelTag="false" />
			</th>
			<th id='<%# Resources.GroupResources.GroupViewReplyCountLabel %>'>
				<cy:SiteLabel id="lblTotalRepliesCountLabel" runat="server" ConfigKey="GroupViewReplyCountLabel" ResourceFile="GroupResources" UseLabelTag="false" />
			</th >
			<th id='<%# Resources.GroupResources.GroupViewPostLastPostLabel %>'>
				<cy:SiteLabel id="lblLastPostLabel" runat="server" ConfigKey="GroupViewPostLastPostLabel" ResourceFile="GroupResources" UseLabelTag="false" />	
			</th>
		</tr></thead>
<asp:Repeater id="rptGroups" runat="server" >
    <HeaderTemplate><tbody></HeaderTemplate>
	<ItemTemplate>
		<tr class="modulerow">
			<td headers='<%# Resources.GroupResources.GroupViewSubjectLabel %>'> 
			    <img alt="" src='<%# ImageSiteRoot + "/Data/SiteImages/" + TopicImage %>'  />
					<asp:HyperLink id="editLink" 
					Text="<%# Resources.GroupResources.GroupTopicEditLabel %>" 
					Tooltip="<%# Resources.GroupResources.GroupTopicEditLabel %>"
					ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
					NavigateUrl='<%# SiteRoot + "/Groups/EditTopic.aspx?topic=" + DataBinder.Eval(Container.DataItem,"TopicID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString()  %>' 
					Visible='<%# GetPermission(DataBinder.Eval(Container.DataItem,"StartedByUserID"))%>' runat="server" />
					
					<asp:HyperLink id="HyperLink3" runat="server"
		            Text="RSS" Tooltip="RSS"
		            ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + RSSImageFileName  %>' 
		            NavigateUrl='<%# SiteRoot + "/RSS.aspx?ItemID=" + ItemId.ToString() + "&mid=" + ModuleId.ToString() + "&pageid=" + PageId.ToString() + "&topic=" + DataBinder.Eval(Container.DataItem,"TopicID") %>' 
		            Visible="<%# EnableRssAtTopicLevel %>"  />
					
					<a href="Topic.aspx?pageid=<%# PageId %>&amp;mid=<%# ModuleId %>&amp;ItemID=<%# ItemId %>&amp;topic=<%# DataBinder.Eval(Container.DataItem,"TopicID") %>">
					<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "TopicTitle").ToString())%></a>
					
			</td>
			<td headers='<%# Resources.GroupResources.GroupViewStartedByLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"StartedBy") %>
			</td>
			<td headers='<%# Resources.GroupResources.GroupViewViewCountLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"TotalViews") %>
			</td>
			<td headers='<%# Resources.GroupResources.GroupViewReplyCountLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"TotalReplies") %>
			</td>
			<td headers='<%# Resources.GroupResources.GroupViewPostLastPostLabel %>'>  
				<%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem),"MostRecentPostDate", TimeOffset) %>
				<br /><%# DataBinder.Eval(Container.DataItem,"MostRecentPostUser") %>
			</td>
		</tr>
	</ItemTemplate>
	<alternatingItemTemplate>
		<tr class="modulealtrow">
			<td  headers='<%# Resources.GroupResources.GroupViewSubjectLabel %>'> 
			    <img alt="" src='<%# ImageSiteRoot + "/Data/SiteImages/" + TopicImage  %>'  />
				<asp:HyperLink id="editLink" 
				Text="<%# Resources.GroupResources.GroupTopicEditLabel %>" 
				Tooltip="<%# Resources.GroupResources.GroupTopicEditLabel %>"
				ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
				NavigateUrl='<%# SiteRoot + "/Groups/EditTopic.aspx?topic=" + DataBinder.Eval(Container.DataItem,"TopicID") + "&mid=" + ModuleId + "&pageid=" + PageId.ToString()  %>' 
				Visible='<%# GetPermission(DataBinder.Eval(Container.DataItem,"StartedByUserID"))%>' 
				runat="server" />
			    <asp:HyperLink id="HyperLink3" runat="server"
		            Text="RSS" Tooltip="RSS"
		            ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + RSSImageFileName  %>' 
		            NavigateUrl='<%# SiteRoot + "/Groups/RSS.aspx?ItemID=" + ItemId.ToString() + "&mid=" + ModuleId + "&pageid=" + PageId.ToString() + "&topic=" + DataBinder.Eval(Container.DataItem,"TopicID") %>' 
		            Visible="<%# EnableRssAtTopicLevel %>"  />
				<a href="Topic.aspx?pageid=<%# PageId %>&amp;mid=<%# ModuleId %>&amp;ItemID=<%# ItemId %>&amp;topic=<%# DataBinder.Eval(Container.DataItem,"TopicID") %>">
					<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "TopicTitle").ToString())%></a>
			</td>
			<td headers='<%# Resources.GroupResources.GroupViewStartedByLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"StartedBy") %>
			</td>
			<td headers='<%# Resources.GroupResources.GroupViewViewCountLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"TotalViews") %>
			</td>
			<td headers='<%# Resources.GroupResources.GroupViewReplyCountLabel %>'>  
				<%# DataBinder.Eval(Container.DataItem,"TotalReplies") %>
			</td>
			<td headers='<%# Resources.GroupResources.GroupViewPostLastPostLabel %>'>  
				<%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.Common.DbDataRecord)Container.DataItem),"MostRecentPostDate", TimeOffset) %>
				<br /><%# DataBinder.Eval(Container.DataItem,"MostRecentPostUser") %>
			</td>
		</tr>
	</AlternatingItemTemplate>
	<FooterTemplate></tbody></FooterTemplate>
</asp:Repeater>
	</table>
    <div class="modulepager">
		<portal:CCutePager ID="pgrBottom" runat="server" EnableViewState="false" />
		<a href="" class="ModulePager" id="lnkNewTopicBottom" runat="server" EnableViewState="false"></a>
    </div>
    <div class="modulefooter">
        &nbsp;
    </div>
</div>
       </portal:CPanel>
       <div class="cleared"></div>
</asp:Panel>
<cy:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />
</portal:CPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />