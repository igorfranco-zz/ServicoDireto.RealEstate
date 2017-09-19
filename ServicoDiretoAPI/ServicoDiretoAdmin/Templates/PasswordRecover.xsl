<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html>
      <head>
        <title><xsl:value-of select="Root/Data/SiteName"/> - Password Assistance</title>
      </head>
      <body>
        <span>
          We received a request to reset the password associated with this e-mail address. If you made this request, please follow the instructions below. <br />
          Click the link below to reset your password using our secure server:<br />          
          <a>
            <xsl:attribute name="href">
              <xsl:value-of select="Root/Data/Site"/>/account.aspx/changepassword?code=<xsl:value-of select="Root/Data/ActivateKey"/>
            </xsl:attribute>
            <xsl:value-of select="Root/Data/Site"/>/account.aspx/changepassword?code=<xsl:value-of select="Root/Data/ActivateKey"/>
          </a>
          <br />
          If you did not request to have your password reset you can safely ignore this email. Rest assured your customer account is safe. <br />
          If clicking the link doesn't seem to work, you can copy and paste the link into your browser's address window, or retype it there. Once you have returned to pescarimoveis.com.br, we will give instructions for resetting your password. <br />
          <xsl:value-of select="Root/Data/SiteName"/> will never e-mail you and ask you to disclose or verify your <xsl:value-of select="Root/Data/SiteName"/> password, credit card, or banking account number. <br />
          If you receive a suspicious e-mail with a link to update your account information, do not click on the link--instead, report the e-mail to <xsl:value-of select="Root/Data/SiteName"/> for investigation. <br />
          Thanks for visiting pescarimoveis.com.br <br />
          <br/><br/>-------------------    Mensagem automática de sistema, favor não responder -----------------------------
        </span>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
