SELECT S.Surname AS "ФАМИЛИЯ", S.Name AS "ИМЯ", SUM(SA.Quantity) AS "ОБЬЕМ ПРОДАЖ"
FROM Sellers AS S
         JOIN Sales AS SA ON S.ID = SA.IDSel
WHERE SA.Date >= '2013-10-01' AND SA.Date <= '2013-10-07'
GROUP BY S.Surname, S.Name
ORDER BY S.Surname, S.Name;

SELECT P.name AS "наименование продукта", S.surname AS "ФАМИЛИЯ", S.name AS "ИМЯ", sum(SA.quantity/A.quantity) * 100 AS "ОБЬЕМ ПРОДАЖ"
FROM sellers AS S
JOIN sales AS SA on S.id = SA.idsel
JOIN products AS P on SA.idprod = P.id 
JOIN arrivals AS A on P.id = A.idprod
WHERE SA.Date >= '2013-10-01' AND SA.Date <= '2013-10-07'
GROUP BY P.name, S.surname, S.name, A.quantity
ORDER BY P.name, S.surname, S.name;