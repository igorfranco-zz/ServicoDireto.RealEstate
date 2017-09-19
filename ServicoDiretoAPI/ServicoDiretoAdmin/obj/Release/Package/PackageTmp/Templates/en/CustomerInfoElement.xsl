<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html>
      <head>
        <title>Solicitação de informação de Imóvel</title>
      </head>
      <body>
        <h3>Solicitação de informação de Imóvel</h3>
        <span>
          Detalhe:
          <a>
            <xsl:attribute name="href">
              <xsl:value-of select="Root/Data/Site"/>/#/property-detail?idElement=<xsl:value-of select="Root/Data/IDElement"/>
            </xsl:attribute>
            <xsl:value-of select="Root/Data/IDElement"/>
          </a>
          <br/>
          <xsl:value-of select="Root/Data/Title"/> <br/>
          <xsl:value-of select="Root/Data/Description"/> <br/>
          <hr />
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
