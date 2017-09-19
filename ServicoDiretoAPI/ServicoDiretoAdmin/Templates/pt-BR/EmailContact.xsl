<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html>
      <head>
        <title>Contato pelo <xsl:value-of select="Root/Data/SiteName"/>
      </title>
      </head>
      <body>
        <h3>
          Contato pelo <xsl:value-of select="Root/Data/SiteName"/>
        </h3>
        <span>
          Solicitante:<br/>
          Nome: <xsl:value-of select="Root/Data/Name"/> <br/>
          E-mail: <xsl:value-of select="Root/Data/Email"/> <br/>
          Mensagem:<br/>
          <xsl:value-of select="Root/Data/Message"/> <br/>
          <hr />
        </span>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
