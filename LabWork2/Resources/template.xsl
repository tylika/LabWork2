<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="html" indent="yes" encoding="UTF-8" />

	<!-- HTML-шаблон -->
	<xsl:template match="/">
		<html>
			<head>
				<title>Результати пошуку</title>
			</head>
			<body>
				<h1>Результати пошуку</h1>
				<table border="1">
					<thead>
						<tr>
							<th>Ім'я</th>
							<th>Прізвище</th>
							<th>Факультет</th>
							<th>Курс</th>
							<th>Кімната</th>
							<th>Дата заселення</th>
							<th>Дата виселення</th>
						</tr>
					</thead>
					<tbody>
						<xsl:for-each select="Results/Person">
							<tr>
								<td>
									<xsl:value-of select="Name/FirstName" />
								</td>
								<td>
									<xsl:value-of select="Name/LastName" />
								</td>
								<td>
									<xsl:value-of select="Faculty" />
								</td>
								<td>
									<xsl:value-of select="Course" />
								</td>
								<td>
									<xsl:value-of select="Room" />
								</td>
								<td>
									<xsl:value-of select="Dates/CheckInDate" />
								</td>
								<td>
									<xsl:value-of select="Dates/CheckOutDate" />
								</td>
							</tr>
						</xsl:for-each>
					</tbody>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
