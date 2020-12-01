using NUnit.Framework;
using WindowsillSoft.CodeChallenges.AdventOfCode._2020;

namespace WindowsillSoft.CodeChallenges.AdventOfCode.Tests.AoC2020
{
    public class Day01Test : TestBase<Day01>
    {
        [Test,
            TestCase("1721\n979\n366\n299\n675\n1456", "514579")]
        public void Part1(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart1(), Is.EqualTo(result));
        }

        [Test,
            TestCase("1721\n979\n366\n299\n675\n1456", "241861950")]
        public void Part2(string input, string result)
        {
            var solver = GetSolver(input);

            Assert.That(solver.ExecutePart2(), Is.EqualTo(result));
        }
        [Test]
        public void FullRun()
        {
            var solver = GetSolver("1686\n1983\n1801\n1890\n1910\n1722\n1571\n1952\n1602\n1551\n1144\n1208\n1335\n1914\n1656\n1515\n1600\n1520\n1683\n1679\n1800\n1889\n1717\n1592\n1617\n1756\n1646\n1596\n1874\n1595\n1660\n1748\n1946\n1734\n1852\n2006\n1685\n1668\n1607\n1677\n403\n1312\n1828\n1627\n1925\n1657\n1536\n1522\n1557\n1636\n1586\n1654\n1541\n1363\n1844\n1951\n1765\n1872\n696\n1764\n1718\n1540\n1493\n1947\n1786\n1548\n1981\n1861\n1589\n1707\n1915\n1755\n1906\n1911\n1628\n1980\n1986\n1780\n1645\n741\n1727\n524\n1690\n1732\n1956\n1523\n1534\n1498\n1510\n372\n1777\n1585\n1614\n1712\n1650\n702\n1773\n1713\n1797\n1691\n1758\n1973\n1560\n1615\n1933\n1281\n1899\n1845\n1752\n1542\n1694\n1950\n1879\n1684\n1809\n1988\n1978\n1843\n1730\n1377\n1507\n1506\n1566\n935\n1851\n1995\n1796\n1900\n896\n171\n1728\n1635\n1810\n2003\n1580\n1789\n1709\n2007\n1639\n1726\n1537\n1976\n1538\n1544\n1626\n1876\n1840\n1953\n1710\n1661\n1563\n1836\n1358\n1550\n1112\n1832\n1555\n1394\n1912\n1884\n1524\n1689\n1775\n1724\n1366\n1966\n1549\n1931\n1975\n1500\n1667\n1674\n1771\n1631\n1662\n1902\n1970\n1864\n2004\n2010\n504\n1714\n1917\n1907\n1704\n1501\n1812\n1349\n1577\n1638\n1886\n1157\n1761\n1676\n1731\n2001\n1261\n1154\n1769\n1529");
            Assert.That("651651", Is.EqualTo(solver.ExecutePart1()));
            Assert.That("214486272", Is.EqualTo(solver.ExecutePart2()));
        }

    }
}

