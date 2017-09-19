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
          Nós recebemos uma requisão para resetar a senha associada a este endereço de email. Se você fez esta requisição, por favor siga as instruções abaixo.<br />
          Clique no endereço abaixo para resetar sua senha usando o nosso servidor seguro<br />
          <a>
            <xsl:attribute name="href">
              <xsl:value-of select="Root/Data/Site"/>/#/change-password/<xsl:value-of select="Root/Data/ActivateKey"/>
            </xsl:attribute>
            <xsl:value-of select="Root/Data/Site"/>/#/change-password/<xsl:value-of select="Root/Data/ActivateKey"/>
          </a>
          <br />
          Se você não fez a requisição para resetar a sua senha você pode seguramente ignorar este email. Fique tranquilo que a sua conta está segura<br />
          Se clicando no endereço não funcionar, você pode copiar e colar o endereço no seu navegador, ou reescrever ele lá.<br />
          Orbigado por visitar <xsl:value-of select="Root/Data/SiteName"/>
          <br/><br/>-------------------    Mensagem automática de sistema, favor não responder -----------------------------
        </span>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
