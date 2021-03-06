﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace XOProject.Controller
{
    [Route("api/Trade/")]
    public class TradeController : ControllerBase
    {
        private IShareRepository _shareRepository { get; set; }
        private ITradeRepository _tradeRepository { get; set; }
        private IPortfolioRepository _portfolioRepository { get; set; }

        public TradeController(IShareRepository shareRepository, ITradeRepository tradeRepository, IPortfolioRepository portfolioRepository)
        {
            _shareRepository = shareRepository;
            _tradeRepository = tradeRepository;
            _portfolioRepository = portfolioRepository;
        }


        [HttpGet("{portfolioid}")]
        public async Task<IActionResult> GetAllTradings([FromRoute]int portFolioid)
        {
            var trade = _tradeRepository.Query().Where(x => x.PortfolioId.Equals(portFolioid));
            return Ok(trade);
        }


        /// <summary>
        /// For a given symbol of share, get the statistics for that particular share calculating the maximum, minimum, 
        /// average and Sum of all the trades for that share individually grouped into Buy trade and Sell trade.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>

        [HttpGet("Analysis/{symbol}")]
        public async Task<IActionResult> GetAnalysis([FromRoute]string symbol)
        {            
            var result = new List<TradeAnalysis>();

            var stats = _tradeRepository.Query()
                .Where(x => x.Symbol == symbol)
                .GroupBy(x => x.Action)
                .Select(x => new
                {                    
                    sumTrades = x.Sum(z => z.NoOfShares),
                    avgTrades = x.Average(z => z.NoOfShares),
                    maxValue = x.Max(z => z.NoOfShares),
                    minValue = x.Min(z => z.NoOfShares),
                    actionValue = x.Key
                }).ToList();

            foreach (var item in stats)
            {
                TradeAnalysis ta = new TradeAnalysis();
                ta.Sum = item.sumTrades;
                ta.Average = item.avgTrades;
                ta.Maximum = item.maxValue;
                ta.Minimum = item.minValue;
                ta.Action = item.actionValue;
                result.Add(ta);
            }

            return Ok(result);
        }
    }
}
