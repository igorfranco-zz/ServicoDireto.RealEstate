<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <title>Recuperação de usuário</title>
      </head>
      <body>
        <h1>Recuperação de usuário</h1>
        <p>
          Usuário:<xsl:value-of select="Root/Data/UserName"/><br />
        </p>
        <p>
          -------------------    Mensagem automática de sistema, favor não responder -----------------------------
        </p>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
