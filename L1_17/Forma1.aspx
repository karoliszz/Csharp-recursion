<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forma1.aspx.cs" Inherits="L1_17.Forma1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="height: 594px">

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="netinkami matmenys" Visible="False"></asp:Label>
            <br />
            <asp:Label ID="Label1" runat="server" Text="Aukstis"></asp:Label>
            <br />
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1" ErrorMessage="iveskite auksti" ForeColor="Red">*</asp:RequiredFieldValidator>
            <br />
            <br />
            <asp:Label ID="Label2" runat="server" Text="Ilgis"></asp:Label>
            <br />
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox2" ErrorMessage="iveskite ilgi" ForeColor="Red">*</asp:RequiredFieldValidator>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Generuoti" />
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Valyti" />
            
            <asp:Table runat="server" ID="Table4"></asp:Table>
            <br />
            <asp:Table ID="Table3" runat="server">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server"> <asp:Table ID="Table1" runat="server" BorderColor="Black" BorderStyle="Solid" GridLines="Both"> </asp:Table></asp:TableCell>
                    <asp:TableCell runat="server"></asp:TableCell>
                    <asp:TableCell runat="server"> <asp:Table ID="Table2" runat="server" BorderColor="Black" BorderStyle="Solid" GridLines="Both"> </asp:Table></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
            <asp:Label ID="Label4" runat="server" Text="Label" Visible="False"></asp:Label>
            
            
            <br />
            
            
&nbsp;&nbsp;&nbsp;

        </div>
    </form>
</body>
</html>
