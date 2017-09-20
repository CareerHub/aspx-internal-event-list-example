<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Event_List_Example.Default" %>

<!DOCTYPE html>

<html>
<body>
    <% foreach (var @event in events) { %>
        <h1>
            <a href="<%= baseUrl + "students/events/detail/" + @event.EntityID %>">
                <%= @event.Name %>
            </a>
        </h1>
        <p><%= @event.Summary %></p>
        <ul>
            <li>Starts: <%= @event.Start.ToShortDateString() %></li>
            <li>Ends: <%= @event.End.ToShortDateString() %></li>
            <li>Venue: <%= @event.OffCampusVenue != null ? @event.OffCampusVenue : $"{@event.Location} {@event.Building} {@event.Campus}" %></li>
        </ul>
        <hr />
    <% } %>
</body>
</html>
