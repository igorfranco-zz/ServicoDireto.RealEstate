<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html>
      <head>
        <title>E-mail de confirmação de cadastro</title>
      </head>
      <body>
        <span>
          <br/>Olá <xsl:value-of select="Root/Data/UserName"/>,
          <br/>Seja bem-vindo ao site Pescar Imóveis.
          <br/>Clique
          <a>
            <xsl:attribute name="href">
              <xsl:value-of select="Root/Data/Site"/>/account.aspx/enable?code=<xsl:value-of select="Root/Data/ActivateKey"/>
            </xsl:attribute>
            Aqui
          </a> para confirma o seu cadastro.
          <br/><br/>-------------------    Mensagem automática de sistema, favor não responder -----------------------------
        </span>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
