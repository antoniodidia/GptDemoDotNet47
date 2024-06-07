<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeFile="Chat.aspx.cs" Inherits="ChatG" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <meta name="author" content="Antonio Di Dia - https://www.antoniodidia.it/"/>
    <title>ChatGPT Demo Integration API using .Net Framework 4.7/4.8</title>
    <style>

        body{
            font-family: Verdana, sans-serif;
        }

        input[type="text"] {
          width: 200px;
          padding: 10px;
          border: 1px solid #ccc;
          border-radius: 5px;
          font-size: 16px;
          outline: none;
        }

        input[type="submit"] {
          width: 100px;
          padding: 10px;
          border: 1px solid #ccc;
          border-radius: 5px;
          font-size: 16px;
          outline: none;
        }

        input[type="submit"]:hover {
          cursor: pointer;
        }

        input[type="submit"]:active {
          background-color: #d8d8d8;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>ChatGPT Demo Integration API using .Net Framework 4.7/4.8</h2>
            <asp:Literal ID="ltChat" runat="server"></asp:Literal>
            <br />
            <asp:TextBox ID="txtMessage" runat="server" required="required"></asp:TextBox>
            <asp:Button ID="btnSend" runat="server" Text="Send" OnClick="BtnSend_Click" />
        </div>
    </form>
</body>
<script>
	if (window.history.replaceState) {
		window.history.replaceState(null, null, window.location.href);
	}
</script>
</html>
