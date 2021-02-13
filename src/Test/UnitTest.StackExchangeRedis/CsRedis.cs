using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetPro.CsRedis;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.System.Text.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.StackExchangeRedis
{
    [TestClass]
    public class CsRedis
    {
        IRedisManager _redisDatabase;

        [TestInitialize]
        public void Init()
        {
            List<string> csredisConns = new List<string>();
            string password = "netpro";
            int defaultDb = 0;
            string ssl = "";
            string keyPrefix = "";
            int writeBuffer = 10240;
            int poolsize = 20;
            int timeout = 10;
            csredisConns.Add($"198.185.15.16:6379,password={password},defaultDatabase={defaultDb},poolsize={poolsize},ssl={ssl},writeBuffer={writeBuffer},prefix={keyPrefix},preheat={1},idleTimeout={timeout},testcluster={true}");

            CSRedis.CSRedisClient csredis;

            try
            {
                csredis = new CSRedis.CSRedisClient(null, csredisConns.ToArray());
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"�����Ƿ�Ϊ������ģʽ,Password����Ϊ���ַ���;����Database�Ƿ�Ϊ0,ֻ���ڷǼ�Ⱥģʽ�²ſ�����Database����0��{ex}");
            }

            RedisHelper.Initialization(csredis);

            _redisDatabase = new CsRedisManager(new RedisCacheOption
            {

            }, new NetPro.CsRedis.SystemTextJsonSerializer());
        }
        [TestMethod]
        public void SetAndExists()
        {
            string key = "6";
            var result = _redisDatabase.Set(key, "����", TimeSpan.FromSeconds(60));
            //var result = RedisHelper.Set(key, "����", TimeSpan.FromSeconds(60));
            var isExists = _redisDatabase.Exists(key);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Exists()
        {
            string key = "6";
            var isExists = _redisDatabase.Exists(key);
            Assert.AreEqual(true, isExists);
        }

        [TestMethod]
        public async Task GetAsync()
        {
            string key = "6";
            var result = _redisDatabase.Set(key, "����", TimeSpan.FromSeconds(60));
            var value = await _redisDatabase.GetAsync<string>(key);
            Assert.AreEqual("����", value);
        }

        [TestMethod]
        public void SetNoTime()
        {
            string key = "SetNoTime";
            var result = _redisDatabase.Set(key, $"{Guid.NewGuid()}");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task StringIncrement()
        {
            string key = "StringIncrement";
            var result = await _redisDatabase.StringIncrementAsync(key, 2);
            var value = await _redisDatabase.GetAsync<long>(key);
            Assert.AreEqual(result, value);
        }

        [TestMethod]
        public async Task CSStringIncrement()
        {
            string key = "CSStringIncrement";
            var result = await _redisDatabase.StringIncrementAsync(key, 1, TimeSpan.FromDays(1));
            var value = await _redisDatabase.GetAsync<long>(key);
            Assert.AreEqual(result, value);
        }

        [TestMethod]
        public async Task Remove()
        {
            string key = "StringIncrement";
            var stringIncrement = _redisDatabase.StringIncrement(key);
            var result = await _redisDatabase.RemoveAsync(key);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Ttl()
        {
            string key = "StringIncrement";
            var result = RedisHelper.Ttl(key);
            Assert.AreNotEqual(0, result);
        }

        [TestMethod]
        public void GetOrSet()
        {
            string key = "GetOrSet";
            var value = Guid.NewGuid().ToString();
            var result = _redisDatabase.GetOrSet<string>(key, () => { return value; });
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public async Task GetOrSetAsync()
        {
            string key = "GetOrSetAsync";
            var value = Guid.NewGuid().ToString();
            var result = await _redisDatabase.GetOrSetAsync<string>(key, async () => { return await Task.FromResult(value); });
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void ZAdd()
        {
            string key = "Zdd";
            _redisDatabase.Remove(key);
            var result = _redisDatabase.SortedSetAdd<dynamic>(key, new { Id = 1, Name = "zheshi" }, (decimal)1);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task ZAddAsync()
        {
            string key = "Zdd";
            _redisDatabase.Remove(key);
            var result = await _redisDatabase.SortedSetAddAsync<dynamic>(key, new { Id = 1, Name = "ZAddAsync" }, (decimal)2);
            Assert.AreEqual(1, result);
        }

    }
}
