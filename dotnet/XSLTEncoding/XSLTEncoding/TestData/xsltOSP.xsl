<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="text" version="1.0" encoding="windows-1250" omit-xml-declaration="yes" />

<xsl:decimal-format
     decimal-separator="."
     grouping-separator=","
/>
<!-- formatovani pro castky obecne -->
<xsl:template name="currency">
<xsl:param name="val" />
<xsl:choose>
<xsl:when test="number(translate($val, ',','.'))">
 <xsl:value-of select="format-number(translate($val, ',','.'), '0')" />
 </xsl:when>
 <xsl:otherwise>0</xsl:otherwise>
</xsl:choose>
</xsl:template>
<!-- formatovani pro rpsn -->
<xsl:template name="rpsn">
<xsl:param name="val" />
<xsl:choose>
<xsl:when test="number(translate($val, ',','.'))">
 <xsl:value-of select="format-number(translate($val, ',','.'), '0.0')" />
 </xsl:when>
 <xsl:otherwise>0.0</xsl:otherwise>
</xsl:choose>
</xsl:template>
<!-- formatovani pro rus -->
<xsl:template name="rus">
<xsl:param name="val" />
<xsl:choose>
<xsl:when test="number(translate($val, ',','.'))">
 <xsl:value-of select="format-number(translate($val, ',','.'), '0.00')" />
 </xsl:when>
 <xsl:otherwise>0.00</xsl:otherwise>
</xsl:choose>
</xsl:template>

  <!-- header -->
<xsl:template match="//PRINT_AGENCY_EXPORT"><xsl:text>1|Home Credit a. s.|Nové sady 996/25|602 00|Brno</xsl:text>
<xsl:apply-templates select="CONTRACTS/CONTRACT" /><xsl:text>&#13;&#10;</xsl:text>
</xsl:template>

  <!-- smlouva -->
<xsl:template match="CONTRACT"><xsl:text>&#13;&#10;</xsl:text><xsl:text>*</xsl:text><xsl:text>&#13;&#10;</xsl:text>
<xsl:text>2|</xsl:text>
<xsl:value-of select="EVID_SRV"/><xsl:text>|</xsl:text>
<xsl:value-of select="IDENT"/><xsl:text>|</xsl:text>
<xsl:value-of select="HONOUR" /><xsl:text>|</xsl:text>
<xsl:value-of select="NAME1" /><xsl:text>|</xsl:text>
<xsl:value-of select="NAME2"/><xsl:text>|</xsl:text>
<xsl:value-of select="IDENT_CARD"/><xsl:text>|</xsl:text>
<xsl:value-of select="STREET" /><xsl:text>|</xsl:text>
<xsl:value-of select="STREET_NUM" /><xsl:text>|</xsl:text>
<xsl:value-of select="TOWN" /><xsl:text>|</xsl:text>
<xsl:value-of select="ZIP_CODE" /><xsl:text>|</xsl:text>
<xsl:choose>
<xsl:when test="STREET1 = ''">
<xsl:value-of select="STREET" /><xsl:text>|</xsl:text>
<xsl:value-of select="STREET_NUM" /><xsl:text>|</xsl:text>
<xsl:value-of select="TOWN" /><xsl:text>|</xsl:text>
<xsl:value-of select="ZIP_CODE" /><xsl:text>|</xsl:text>
</xsl:when>
<xsl:otherwise>
<xsl:value-of select="STREET1" /><xsl:text>|</xsl:text>
<xsl:value-of select="STREET_NUM1" /><xsl:text>|</xsl:text>
<xsl:value-of select="TOWN1" /><xsl:text>|</xsl:text>
<xsl:value-of select="ZIP_CODE1" /><xsl:text>|</xsl:text>
</xsl:otherwise>
</xsl:choose>
<xsl:value-of select="PHONE" /><xsl:text>|</xsl:text>
<xsl:value-of select="MOBIL" /><xsl:text>|</xsl:text>
<xsl:value-of select="EMAIL" /><xsl:text>|</xsl:text>
<xsl:value-of select="FAM_STATUS" /><xsl:text>|</xsl:text>
<xsl:value-of select="HOUSE_TYPE"/><xsl:text>|</xsl:text>
<xsl:value-of select="EDUCATION"/><xsl:text>|</xsl:text>
<xsl:value-of select="INCOME_TYPE" /><xsl:text>|</xsl:text>
<xsl:value-of select="EMP_NAME" /><xsl:text>|</xsl:text>
<xsl:value-of select="EMP_IDENT" /><xsl:text>|</xsl:text>
<xsl:value-of select="EMP_PHONE" /><xsl:text>|</xsl:text>
<xsl:value-of select="EMPLOY_FROM/MONTH" /><xsl:text>/</xsl:text>
<xsl:value-of select="EMPLOY_FROM/YEAR" /><xsl:text>|</xsl:text>
<xsl:value-of select="STREET2" /><xsl:text>|</xsl:text>
<xsl:value-of select="STREET_NUM2" /><xsl:text>|</xsl:text>
<xsl:value-of select="TOWN2" /><xsl:text>|</xsl:text>
<xsl:value-of select="ZIP_CODE2" /><xsl:text>|</xsl:text>
<xsl:call-template name="currency"><xsl:with-param name="val" select="INCOME" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:call-template name="currency"><xsl:with-param name="val" select="CREDIT_AMOUNT" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:call-template name="currency"><xsl:with-param name="val" select="ANNUITY" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:value-of select="PAYMENT_NUM" /><xsl:text>|</xsl:text>
<xsl:call-template name="rpsn"><xsl:with-param name="val" select="RPSN" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:call-template name="currency"><xsl:with-param name="val" select="FEE_CREDIT" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:value-of select="REPAYMENT_TYPE_CODE" /><xsl:text>|</xsl:text>
<xsl:value-of select="SELLERPLACE_CODE" /><xsl:text>|</xsl:text>
<xsl:value-of select="PROD_CODE" /><xsl:text>|</xsl:text>
<xsl:value-of select="AUTCODE_MOVE"/><xsl:text>|</xsl:text>
<xsl:value-of select="ACTION_CODE"/><xsl:text>|</xsl:text>
<xsl:call-template name="currency"><xsl:with-param name="val" select="INCOME_PART" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:value-of select="CHILD_NUM"/><xsl:text>|</xsl:text>
<xsl:call-template name="currency"><xsl:with-param name="val" select="TOTAL_AMOUNT" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:value-of select="PAY_TYPE" /><xsl:text>|</xsl:text>
<xsl:value-of select="NUM_BEFORE" /><xsl:value-of select="NUM_ACCOUNT" /><xsl:text>|</xsl:text>
<xsl:value-of select="BANKCODE" /><xsl:text>|</xsl:text>
<xsl:value-of select="DATE_RATIF/DAY" /><xsl:text>.</xsl:text>
<xsl:value-of select="DATE_RATIF/MONTH" /><xsl:text>.</xsl:text>
<xsl:value-of select="DATE_RATIF/YEAR" /><xsl:text>|</xsl:text>
<xsl:value-of select="//DATE_EXPORT/DAY" /><xsl:text>.</xsl:text>
<xsl:value-of select="//DATE_EXPORT/MONTH" /><xsl:text>.</xsl:text>
<xsl:value-of select="//DATE_EXPORT/YEAR" /><xsl:text>|</xsl:text>
<xsl:apply-templates select="INSURANCES" mode="name" /><xsl:text>|</xsl:text>
<xsl:call-template name="currency"><xsl:with-param name="val" select="ANNUITY_NO_INSUR" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:call-template name="currency"><xsl:with-param name="val" select="INSURANCE_SUM" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:apply-templates select="INSURANCES" mode="risk" /><xsl:text>|</xsl:text>
<xsl:call-template name="currency"><xsl:with-param name="val" select="COSUA" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:value-of select="HC_NUM_BEFORE" /><xsl:value-of select="HC_NUM_ACCOUNT" /><xsl:text>|</xsl:text>
<xsl:value-of select="HC_BANK_CODE" /><xsl:text>|</xsl:text>
<xsl:value-of select="HC_BANK_NAME" /><xsl:text>|</xsl:text>
<xsl:call-template name="rpsn"><xsl:with-param name="val" select="MIN_RPSN" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:call-template name="rpsn"><xsl:with-param name="val" select="MAX_RPSN" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:value-of select="RECALC_ALLOWED_TXT" /><xsl:text>|</xsl:text>
<xsl:call-template name="rus"><xsl:with-param name="val" select="RUS" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:call-template name="currency"><xsl:with-param name="val" select="TOTAL_AMOUNT" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:value-of select="PAPERFORM_NAME" /><xsl:text>|</xsl:text>
<xsl:call-template name="currency"><xsl:with-param name="val" select="ACSUA" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:value-of select="CONSENT_NRKI" /><xsl:text>|</xsl:text>
<xsl:value-of select="IH_ALLOWED" /><xsl:text>|</xsl:text>
<xsl:call-template name="currency"><xsl:with-param name="val" select="IHSUA" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:call-template name="currency"><xsl:with-param name="val" select="BASIC_ANNUITY" /></xsl:call-template><xsl:text>|</xsl:text>
<xsl:value-of select="DOC_DELIVERY_COMB_CODE" /><xsl:text>|</xsl:text>
<xsl:value-of select="BONUS_ACTION_CODE" />
</xsl:template>

<!-- pojisteni -->
<xsl:template match="INSURANCES" mode="name" >
<xsl:for-each select=".//PACKAGE_NAME">
<xsl:value-of select="." />
<xsl:if test="not(position()=last())"><xsl:text>;</xsl:text></xsl:if>
</xsl:for-each>
</xsl:template>

<!-- rizika -->
<xsl:template match="INSURANCES" mode="risk" >
<xsl:for-each select=".//RISK[not(.=preceding::*)]">
<xsl:value-of select="." />
<xsl:if test="not(position()=last())"><xsl:text>;</xsl:text></xsl:if>
</xsl:for-each>
</xsl:template>

<!-- TRACK_DOC -->
<xsl:template match="TRACK_DOCS" >
<xsl:for-each select=".//TRACK_DOC">
<xsl:value-of select="." />
<xsl:if test="not(position()=last())"><xsl:text>|</xsl:text></xsl:if>
</xsl:for-each>
</xsl:template>

</xsl:stylesheet>
