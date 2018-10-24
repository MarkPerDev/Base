using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RRD.RolloutManagement.Data;
using RRD.RolloutManagement.Data.Views;
using RRD.RolloutManagement.Services;
using RRD.RolloutManagement.Services.Interfaces;
using RRD.RolloutManagement.Web.api.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using UnitTest.RolloutManagement.MockDataProviders;

namespace UnitTest.RolloutManagement
{
	[TestClass]
	public class PromotionControllerTest : BaseApiControllerTest
	{
		//private const string RESOURCE_PATH = @"promotion/";
		//private const string CORP_NBR = @"0000000425";

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		[ClassInitialize()]
		public static void MyClassInitialize(TestContext testContext)
		{
			BaseApiControllerTest.ClassInitialize(testContext);
			_resource = @"promotions";
		}
		//
		// Use ClassCleanup to run code after all tests in a class have run
		[ClassCleanup()]
		public static void MyClassCleanup()
		{
			BaseApiControllerTest.ClassCleanup();
		}
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		#region API Service Test Methods

		[TestMethod, TestCategory(Constants.TEST_CATEGORY__API_SERVICE)]
		public void GetByCorpNbrTest()
		{
			var response = _client.GetAsync(ApiPath).Result;
			response.EnsureSuccessStatusCode();

			var list = response.Content.ReadAsAsync<IEnumerable<PromotionDetail>>().Result;
			Assert.IsTrue(list != null && (list.Count() > 0));
		}

		[TestMethod, TestCategory(Constants.TEST_CATEGORY__API_SERVICE)]
		public void GetByCorpNbrAndIdTest()
		{
			int promoId = 4826;
			var response = _client.GetAsync(string.Concat(ApiPath, promoId)).Result;
			response.EnsureSuccessStatusCode();

			var actual = response.Content.ReadAsAsync<PromotionDetail>().Result;
			Assert.IsTrue(actual != null && (actual.promotion_id == promoId) && (actual.ShipSummary == null));
		}

		[TestMethod, TestCategory(Constants.TEST_CATEGORY__API_SERVICE)]
		public void GetByCorpNbrAndIdSummaryTest()
		{
			int promoId = 4826;
			var response = _client.GetAsync(string.Concat(ApiPath, promoId, "/?view=summary")).Result;
			response.EnsureSuccessStatusCode();

			var actual = response.Content.ReadAsAsync<PromotionDetail>().Result;
			Assert.IsTrue(actual != null && (actual.promotion_id == promoId) && (actual.ShipSummary != null));
		}

		[TestMethod, TestCategory(Constants.TEST_CATEGORY__API_SERVICE)]
		public void GetByCorpNbrAndIdFailTest()
		{
			int promoId = 999999999;
			var response = _client.GetAsync(string.Concat(ApiPath, "?promoId=", promoId)).Result;
			Assert.IsFalse(response.IsSuccessStatusCode);

			Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
		}

		[TestMethod, TestCategory(Constants.TEST_CATEGORY__API_SERVICE)]
		public void GetByCorpNbrSortTest()
		{
			string sort = HttpUtility.UrlDecode("promotion_code desc");
			var response = _client.GetAsync(string.Concat(ApiPath, "?limit=0&offset=0&sort=", sort)).Result;
			response.EnsureSuccessStatusCode();

			var results = response.Content.ReadAsAsync<PagedResults<PromotionDetail>>().Result;
			Assert.IsTrue(results != null && (results.PageOfData != null)
				&& (results.TotalRowCount > 0) && (results.PageOfData.Count() > 0));
		}

		[TestMethod, TestCategory(Constants.TEST_CATEGORY__API_SERVICE)]
		public void GetByCorpNbrPagingTest()
		{
			string sort = HttpUtility.UrlDecode("promotion_code desc");
			var response = _client.GetAsync(string.Concat(ApiPath, "?limit=5&offset=0&sort=", sort)).Result;
			response.EnsureSuccessStatusCode();

			var results = response.Content.ReadAsAsync<PagedResults<PromotionDetail>>().Result;
			Assert.IsTrue(results != null && (results.PageOfData != null)
				&& (results.TotalRowCount > 0) && (results.PageOfData.Count() > 0));
		}

		[TestMethod, TestCategory(Constants.TEST_CATEGORY__API_SERVICE)]
		public void GetByCorpNbrDetailPagingTest()
		{
			string sort = HttpUtility.UrlDecode("promotion_code desc");
			var response = _client.GetAsync(string.Concat(ApiPath, "?view=extended&limit=5&offset=0&sort=", sort)).Result;
			response.EnsureSuccessStatusCode();

			var results = response.Content.ReadAsAsync<PagedResults<PromotionDetail>>().Result;
			Assert.IsTrue(results != null && (results.PageOfData != null)
				&& (results.TotalRowCount > 0) && (results.PageOfData.Count() > 0));
		}

		[TestMethod, TestCategory(Constants.TEST_CATEGORY__API_SERVICE)]
		public void GetByCorpNbrSearchPagingTest()
		{
			string sort = HttpUtility.UrlDecode("promotion_code desc");
			string search = HttpUtility.UrlEncode("import");
			var response = _client.GetAsync(string.Concat(ApiPath, "?limit=5&offset=0&sort=", sort, "&keyword=", search)).Result;
			response.EnsureSuccessStatusCode();

			var results = response.Content.ReadAsAsync<PagedResults<PromotionDetail>>().Result;
			Assert.IsTrue(results != null && (results.PageOfData != null)
				&& (results.TotalRowCount > 0) && (results.PageOfData.Count() > 0));
		}

		[TestMethod, TestCategory(Constants.TEST_CATEGORY__API_SERVICE)]
		public void AddAndDeleteTest()
		{
			PromotionDetail newPromo = new PromotionDetail()
			{
				promotion_code = Guid.NewGuid().ToString(),
				promotion_description = "unit test add",
				created_by = "unit test method",
				wip = true
			};
			//newPromo.Items = new List<PromotionItem>() { new PromotionItem() { customer_item_number = "test 1" } };

			var response = _client.PostAsXmlAsync<PromotionDetail>(ApiPath, newPromo).Result;
			Assert.IsTrue(response.IsSuccessStatusCode);

			Assert.IsTrue((response.Headers != null) && (response.Headers.Location != null));
			var response2 = _client.DeleteAsync(response.Headers.Location).Result;
			Assert.IsTrue(response2.IsSuccessStatusCode);
		}

		[TestMethod, TestCategory(Constants.TEST_CATEGORY__API_SERVICE)]
		public void AddUpdateCompleteTest()
		{
			PromotionDetail newPromo = new PromotionDetail()
			{
				promotion_event_id = 1,
				promotion_code = Guid.NewGuid().ToString(),
				attention = string.Empty,
				notes = string.Empty,
				wip = true,
				created_by = "unit test method"
			};

			// make POST request to add the new promo
			var response = _client.PostAsXmlAsync<PromotionDetail>(ApiPath, newPromo).Result;
			Assert.IsTrue(response.IsSuccessStatusCode);

			Assert.IsTrue((response.Headers != null) && (response.Headers.Location != null));

			// make GET request to get the newly added promo
			var uriNewPromo = response.Headers.Location;
			var response2 = _client.GetAsync(uriNewPromo).Result;
			response2.EnsureSuccessStatusCode();

			var actual = response2.Content.ReadAsAsync<PromotionDetail>().Result;
			Assert.IsTrue(actual != null);

			actual.promotion_code = "Code was changed by unit test.";

			List<int> catIds = new List<int>();
			using (RolloutManagementContext dbContext = new RolloutManagementContext())
			{
				// set store selection criteria - use first SSF with has_qty, and its first SSV
				var corpSelectFields = (from ssf in dbContext.store_select_field
										where ssf.customer.wcss_customer_number == CorpNumber
											&& ssf.has_qty == true
										select ssf).Take(1).ToList();

				if (corpSelectFields != null && (corpSelectFields.Count > 0))
				{
					int ssfId = corpSelectFields[0].ssf_id;

					var selectValues = (from ssv in dbContext.store_select_value
										where ssv.ssf_id == ssfId
										select ssv).Take(1).ToList();

					if (selectValues != null && (selectValues.Count > 0))
					{
						StoreSelectCriteriaGroup group1 = new StoreSelectCriteriaGroup();
						StoreSelectCriteriaItem criteria1 = new StoreSelectCriteriaItem()
						{
							ssfID = ssfId,
							ssvID = selectValues[0].ssv_id,
							qty = 2,
							HasQty = true,
							CriteriaField = corpSelectFields[0].name,
							ValueField = selectValues[0].value
						};
						group1.CriteriaItems = new List<StoreSelectCriteriaItem>() { criteria1 };
						actual.CriteriaGroups = new List<StoreSelectCriteriaGroup>() { group1 };
					}
				}

				// set categories and items
				var corpCatIds = (from c in dbContext.category
								  where c.customer.wcss_customer_number.Equals(CorpNumber,
									  StringComparison.InvariantCulture)
								  select c.category_id).Take(1).ToList();
				if ((corpCatIds != null) && (corpCatIds.Count > 0))
				{
					catIds.Add(corpCatIds[0]);
					//actual.CategoryIds = catIds;
				}
			}

			actual.Items = new List<PromotionItem>() { new PromotionItem() { customer_item_number = "test 1", CategoryIds = catIds } };
			actual.wip = false;

			// make PUT request to update the new promo
			var response3 = _client.PutAsXmlAsync<PromotionDetail>(ApiPath, actual).Result;
			Assert.IsTrue(response3.IsSuccessStatusCode);

			//// make DELETE request to delete the new promo
			//var response4 = _client.DeleteAsync(uriNewPromo).Result;
			//Assert.IsTrue(response4.IsSuccessStatusCode);
		}

		[TestMethod, TestCategory(Constants.TEST_CATEGORY__API_SERVICE)]
		public void AddWithStoresCompleteTest()
		{
			PromotionDetail newPromo = new PromotionDetail()
			{
				promotion_code = Guid.NewGuid().ToString(),
				attention = string.Empty,
				notes = string.Empty,
				wip = true,
				created_by = "unit test method"
			};

			// make POST request to add the new promo
			var response = _client.PostAsXmlAsync<PromotionDetail>(ApiPath, newPromo).Result;
			Assert.IsTrue(response.IsSuccessStatusCode);

			Assert.IsTrue((response.Headers != null) && (response.Headers.Location != null));

			// make GET request to get the newly added promo
			var uriNewPromo = response.Headers.Location;
			var response2 = _client.GetAsync(uriNewPromo).Result;
			response2.EnsureSuccessStatusCode();

			var actual = response2.Content.ReadAsAsync<PromotionDetail>().Result;
			Assert.IsTrue(actual != null);

			actual.promotion_code = "Code was changed by unit test.";

			List<int> catIds = new List<int>();
			using (RolloutManagementContext dbContext = new RolloutManagementContext())
			{
				// set store selection criteria - use first 5 active stores
				var corpStores = (from s in dbContext.store
								  where s.customer.wcss_customer_number == CorpNumber
								  && s.active == true && s.closed == false
								  select s).Take(5).ToList();

				if (corpStores != null && (corpStores.Count > 0))
				{
					StoreSelectCriteriaGroup group1 = new StoreSelectCriteriaGroup() { HasStoreIDs = true };
					group1.CriteriaItems = new List<StoreSelectCriteriaItem>();

					foreach (store curStore in corpStores)
					{
						StoreSelectCriteriaItem criteria1 = new StoreSelectCriteriaItem()
						{
							ssfID = ConversionService.StoreIdSsfId, // -66
							ssvID = -1,
							qty = 2,
							HasQty = true,
							CriteriaField = "store_id",
							FieldNameHidden = "store_id",
							ValueNameHidden = curStore.store_id.ToString()
						};
						group1.CriteriaItems.Add(criteria1);
					}
					actual.CriteriaGroups = new List<StoreSelectCriteriaGroup>() { group1 };
				}

				// set categories and items
				var corpCatIds = (from c in dbContext.category
								  where c.customer.wcss_customer_number == CorpNumber
								  select c.category_id).Take(1).ToList();
				if ((corpCatIds != null) && (corpCatIds.Count > 0))
				{
					catIds.Add(corpCatIds[0]);
					//actual.CategoryIds = catIds;
				}
			}

			var respUpd = _client.PutAsXmlAsync<PromotionDetail>(ApiPath, actual).Result;
			Assert.IsTrue(respUpd.IsSuccessStatusCode);
			actual = respUpd.Content.ReadAsAsync<PromotionDetail>().Result;

			Assert.IsTrue(actual != null);

			actual.Items = new List<PromotionItem>() { new PromotionItem() { customer_item_number = "test 1", CategoryIds = catIds } };
			//actual.Items = new List<PromotionItem>() { new PromotionItem() { customer_item_number = "test 1" } };
			//newPromo.wip = true;

			// make PUT request to update the new promo
			var response3 = _client.PutAsXmlAsync<PromotionDetail>(ApiPath, actual).Result;
			Assert.IsTrue(response3.IsSuccessStatusCode);

			//// make DELETE request to delete the new promo
			//var response4 = _client.DeleteAsync(uriNewPromo).Result;
			//Assert.IsTrue(response4.IsSuccessStatusCode);
		}

		//[TestMethod, TestCategory(Constants.TEST_CATEGORY__API_SERVICE)]
		//public void DeleteTest()
		//{
		//	var response = _client.DeleteAsync(string.Concat(API_URI, CORP_NBR, "/?promoId=8")).Result;
		//	response.EnsureSuccessStatusCode();
		//	Assert.IsTrue(true);
		//}

		[TestMethod, TestCategory(Constants.TEST_CATEGORY__API_SERVICE)]
		public void DeleteFailTest()
		{
			var response = _client.DeleteAsync(string.Concat(ApiPath, "?promoId=999999999")).Result;
			Assert.IsFalse(response.IsSuccessStatusCode);

			var errorContainer = response.Content.ReadAsAsync<ErrorContainer>().Result;
			Debug.WriteLine(errorContainer);

			Assert.IsTrue(errorContainer != null);
		}

		#endregion API Service Test Methods

		#region GetPromotion(corpId, promoId) Test Methods

		/// <summary>
		/// Verify that <see cref="PromotionController.Get(string, int, string)"/> calls the
		/// expected service methods and returns the expected promotion.
		/// </summary>
		[TestMethod, TestCategory(Constants.TEST_CATEGORY_CHECK_IN)]
		public void Test_GetPromotion_CorpIdPromoId()
		{
			// Avoid the unit test going to the database to get the customer id.
			PutCustomerIdIntoCache(MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER,
				MockConstants.TEST_CUSTOMER_ID);

			var testPromotion = new PromotionDetail();

			var mockPromotionService = new Mock<IPromotionServices>();

			mockPromotionService.Setup(ps => ps.GetPromotionDetail(MockConstants.TEST_CUSTOMER_ID,
				MockConstants.TEST_PROMOTION_1__ID, null)).Returns(testPromotion);

			var testController = new PromotionController(mockPromotionService.Object);

			var actualPromotion = testController.Get(MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER,
				MockConstants.TEST_PROMOTION_1__ID);

			Assert.AreEqual(testPromotion, actualPromotion,
				"The controller should return the promotion that the service returns.");
		}

		/// <summary>
		/// Verify that <see cref="PromotionController.Get(string, int, string)"/> calls the
		/// expected service methods and returns the expected promotion. Also verifies that the
		/// view parameter is passed to the service.
		/// </summary>
		[TestMethod, TestCategory(Constants.TEST_CATEGORY_CHECK_IN)]
		public void Test_GetPromotion_CorpIdPromoId_ViewItemCount()
		{
			// Avoid the unit test going to the database to get the customer id.
			PutCustomerIdIntoCache(MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER,
				MockConstants.TEST_CUSTOMER_ID);

			var testView = @"itemcount";

			var testPromotion = new PromotionDetail();

			var mockPromotionService = new Mock<IPromotionServices>();

			mockPromotionService.Setup(ps => ps.GetPromotionDetail(MockConstants.TEST_CUSTOMER_ID,
				MockConstants.TEST_PROMOTION_1__ID, testView)).Returns(testPromotion);

			var testController = new PromotionController(mockPromotionService.Object);

			var actualPromotion = testController.Get(MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER,
				MockConstants.TEST_PROMOTION_1__ID, testView);

			Assert.AreEqual(testPromotion, actualPromotion,
				"The controller should return the promotion that the service returns.");
		}

		#endregion GetPromotion(corpId, promoId) Test Methods

		#region GetPromotions(corpId) Test Methods

		/// <summary>
		/// Verify that <see cref="PromotionController.Get(string, int, string)"/> calls the
		/// expected service methods and returns the expected promotion.
		/// </summary>
		[TestMethod, TestCategory(Constants.TEST_CATEGORY_CHECK_IN)]
		public void Test_GetPromotions_CorpId()
		{
			// Avoid the unit test going to the database to get the customer id.
			PutCustomerIdIntoCache(MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER,
				MockConstants.TEST_CUSTOMER_ID);

			var testPromotionList = new List<PromotionDetail>()
			{
				new PromotionDetail()
			};

			var mockPromotionService = new Mock<IPromotionServices>();

			mockPromotionService.Setup(ps => ps.GetPromotionDetailList(
				MockConstants.TEST_CUSTOMER_ID, null)).Returns(testPromotionList);

			var testController = new PromotionController(mockPromotionService.Object);

			var actualPromotionList = testController.Get(MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER);

			Assert.AreEqual(testPromotionList, actualPromotionList,
				"The controller should return the promotion list that the service returns.");
		}

		/// <summary>
		/// Verify that <see cref="PromotionController.Get(string, int, string)"/> calls the
		/// expected service methods and returns the expected promotion. Also verifies that the
		/// view parameter is passed to the service.
		/// </summary>
		[TestMethod, TestCategory(Constants.TEST_CATEGORY_CHECK_IN)]
		public void Test_GetPromotions_CorpId_ViewItemCount()
		{
			// Avoid the unit test going to the database to get the customer id.
			PutCustomerIdIntoCache(MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER,
				MockConstants.TEST_CUSTOMER_ID);

			var testView = @"itemcount";

			var testPromotionList = new List<PromotionDetail>()
			{
				new PromotionDetail()
			};

			var mockPromotionService = new Mock<IPromotionServices>();

			mockPromotionService.Setup(ps => ps.GetPromotionDetailList(
				MockConstants.TEST_CUSTOMER_ID, testView)).Returns(testPromotionList);

			var testController = new PromotionController(mockPromotionService.Object);

			var actualPromotionList = testController.Get(MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER,
				testView);

			Assert.AreEqual(testPromotionList, actualPromotionList,
				"The controller should return the promotion list that the service returns.");
		}

		#endregion GetPromotions(corpId) Test Methods

		#region GetPromotions(corpId, limit, offset, eventId) Test Methods

		/// <summary>
		/// Verify that <see cref="PromotionController.Get(string, int, string)"/> calls the
		/// expected service methods and returns the expected promotion.
		/// </summary>
		[TestMethod, TestCategory(Constants.TEST_CATEGORY_CHECK_IN)]
		public void Test_GetPromotions_CorpIdLimitOffsetEventId()
		{
			// Avoid the unit test going to the database to get the customer id.
			PutCustomerIdIntoCache(MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER,
				MockConstants.TEST_CUSTOMER_ID);

			var testPromotionList = new List<PromotionDetail>()
			{
				new PromotionDetail()
			};

			var mockPromotionService = new Mock<IPromotionServices>();

			int totalPromoCount;
			int limit = 10;
			int offset = 0;
			string filter = null;
			string sort = null;
			string view = null;

			mockPromotionService.Setup(ps => ps.GetPromotionDetailList(
				MockConstants.TEST_CUSTOMER_ID, limit, offset,
				MockConstants.TEST_PROMO_EVENT_1__ID, out totalPromoCount,
				filter, sort, view)).Returns(testPromotionList);

			var testController = new PromotionController(mockPromotionService.Object);

			var actualPageOfPromotions = testController.Get(
				MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER, limit, offset,
				MockConstants.TEST_PROMO_EVENT_1__ID);

			Assert.AreEqual(testPromotionList, actualPageOfPromotions.PageOfData,
				"The controller should return the promotion list that the service returns.");
			Assert.AreEqual(offset, actualPageOfPromotions.RowsSkipped,
				"The controller should return RowsSkipped that equals the offset requested.");
			Assert.AreEqual(limit, actualPageOfPromotions.RowsPerPage,
				"The controller should return RowsPerPage that equals the limit requested.");
		}

		/// <summary>
		/// Verify that <see cref="PromotionController.Get(string, int, string)"/> calls the
		/// expected service methods and returns the expected promotion. Also verifies that the
		/// view parameter is passed to the service.
		/// </summary>
		[TestMethod, TestCategory(Constants.TEST_CATEGORY_CHECK_IN)]
		public void Test_GetPromotions_CorpIdLimitOffsetEventId_ViewItemCount()
		{
			// Avoid the unit test going to the database to get the customer id.
			PutCustomerIdIntoCache(MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER,
				MockConstants.TEST_CUSTOMER_ID);

			var testView = @"itemcount";

			var testPromotionList = new List<PromotionDetail>()
			{
				new PromotionDetail()
			};

			var mockPromotionService = new Mock<IPromotionServices>();

			int totalPromoCount;
			int limit = 10;
			int offset = 0;
			string filter = null;
			string sort = null;

			mockPromotionService.Setup(ps => ps.GetPromotionDetailList(
				MockConstants.TEST_CUSTOMER_ID, limit, offset,
				MockConstants.TEST_PROMO_EVENT_1__ID, out totalPromoCount,
				filter, sort, testView)).Returns(testPromotionList);

			var testController = new PromotionController(mockPromotionService.Object);

			var actualPageOfPromotions = testController.Get(
				MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER, limit, offset,
				MockConstants.TEST_PROMO_EVENT_1__ID, view: testView);

			Assert.AreEqual(testPromotionList, actualPageOfPromotions.PageOfData,
				"The controller should return the promotion list that the service returns.");
			Assert.AreEqual(offset, actualPageOfPromotions.RowsSkipped,
				"The controller should return RowsSkipped that equals the offset requested.");
			Assert.AreEqual(limit, actualPageOfPromotions.RowsPerPage,
				"The controller should return RowsPerPage that equals the limit requested.");
		}

		#endregion GetPromotions(corpId, limit, offset, eventId) Test Methods

		#region GetPromotions(corpId, limit, offset, createdBy, filter, . . . ) Test Methods

		/// <summary>
		/// Verify that <see cref="PromotionController.Get(string, int, string)"/> calls the
		/// expected service methods and returns the expected promotion.
		/// </summary>
		[TestMethod, TestCategory(Constants.TEST_CATEGORY_CHECK_IN)]
		public void Test_GetPromotions_CorpId14Args()
		{
			// Avoid the unit test going to the database to get the customer id.
			PutCustomerIdIntoCache(MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER,
				MockConstants.TEST_CUSTOMER_ID);

			var testPromotionList = new List<PromotionDetail>()
			{
				new PromotionDetail()
			};

			var mockPromotionService = new Mock<IPromotionServices>();

			int totalPromoCount;
			int limit = 10;
			int offset = 0;
			string createdBy = @"unit.test@rrd.com";
			string filter = null;
			string sort = null;
			string view = null;
			string startDate = DateTime.Now.ToString();
			string endDate = DateTime.Now.ToString();
			string activeRangeStartDate = DateTime.Now.ToString();
			string activeRangeEndDate = DateTime.Now.ToString();
			string orderStartDate = DateTime.Now.ToString();
			string orderEndDate = DateTime.Now.ToString();
			string keyword = @"pass";

			mockPromotionService.Setup(ps => ps.GetPromotionDetailList(
				MockConstants.TEST_CUSTOMER_ID, limit, offset, out totalPromoCount, createdBy,
				filter, sort, view, startDate, endDate, activeRangeStartDate, activeRangeEndDate,
				orderStartDate, orderEndDate, keyword))
				.Returns(testPromotionList);

			var testController = new PromotionController(mockPromotionService.Object);

			var actualPageOfPromotions = testController.Get(
				MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER, limit, offset, createdBy,
				filter, sort, view, startDate, endDate, activeRangeStartDate, activeRangeEndDate,
				orderStartDate, orderEndDate, keyword);

			Assert.AreEqual(testPromotionList, actualPageOfPromotions.PageOfData,
				"The controller should return the promotion list that the service returns.");
			Assert.AreEqual(offset, actualPageOfPromotions.RowsSkipped,
				"The controller should return RowsSkipped that equals the offset requested.");
			Assert.AreEqual(limit, actualPageOfPromotions.RowsPerPage,
				"The controller should return RowsPerPage that equals the limit requested.");
		}

		/// <summary>
		/// Verify that <see cref="PromotionController.Get(string, int, string)"/> calls the
		/// expected service methods and returns the expected promotion. Also verifies that the
		/// view parameter is passed to the service.
		/// </summary>
		[TestMethod, TestCategory(Constants.TEST_CATEGORY_CHECK_IN)]
		public void Test_GetPromotions_CorpId14Args_ViewItemCount()
		{
			// Avoid the unit test going to the database to get the customer id.
			PutCustomerIdIntoCache(MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER,
				MockConstants.TEST_CUSTOMER_ID);

			var testView = @"itemcount";

			var testPromotionList = new List<PromotionDetail>()
			{
				new PromotionDetail()
			};

			var mockPromotionService = new Mock<IPromotionServices>();

			int totalPromoCount;
			int limit = 10;
			int offset = 0;
			string createdBy = @"unit.test@rrd.com";
			string filter = null;
			string sort = null;
			string startDate = DateTime.Now.ToString();
			string endDate = DateTime.Now.ToString();
			string activeRangeStartDate = DateTime.Now.ToString();
			string activeRangeEndDate = DateTime.Now.ToString();
			string orderStartDate = DateTime.Now.ToString();
			string orderEndDate = DateTime.Now.ToString();
			string keyword = @"pass";

			mockPromotionService.Setup(ps => ps.GetPromotionDetailList(
				MockConstants.TEST_CUSTOMER_ID, limit, offset, out totalPromoCount, createdBy,
				filter, sort, testView, startDate, endDate, activeRangeStartDate, activeRangeEndDate,
				orderStartDate, orderEndDate, keyword))
				.Returns(testPromotionList);

			var testController = new PromotionController(mockPromotionService.Object);

			var actualPageOfPromotions = testController.Get(
				MockConstants.TEST_CUSTOMER__CORPORATE_NUMBER, limit, offset, createdBy,
				filter, sort, testView, startDate, endDate, activeRangeStartDate, activeRangeEndDate,
				orderStartDate, orderEndDate, keyword);

			Assert.AreEqual(testPromotionList, actualPageOfPromotions.PageOfData,
				"The controller should return the promotion list that the service returns.");
			Assert.AreEqual(offset, actualPageOfPromotions.RowsSkipped,
				"The controller should return RowsSkipped that equals the offset requested.");
			Assert.AreEqual(limit, actualPageOfPromotions.RowsPerPage,
				"The controller should return RowsPerPage that equals the limit requested.");
		}

		#endregion GetPromotions(corpId, limit, offset, createdBy, filter, . . . ) Test Methods

		#region Dispose(bool) Test Methods

		/// <summary>
		/// Verifies that the <see cref="BaseApiController"/> when given a
		/// <see cref="PromotionServices"/> instance at construction does not call
		/// <see cref="IDisposable.Dispose"/> when the controller is disposed.
		/// </summary>
		/// <remarks>
		/// It is assumed that when the <see cref="PromotionServices"/> instance is provided to the
		/// controller's constructor that the consumer of the controller, which is also the provider
		/// of the <see cref="BaseApiController.PromoContext"/>, is responsible for the lifetime of
		/// the service and thus for disposing it. So the controller should not call Dispose on the
		/// context.
		/// </remarks>
		[TestMethod, TestCategory(Constants.TEST_CATEGORY_CHECK_IN)]
		public void Test_ConstructWithPromoContext_DoesNotDisposePromoContext()
		{
			var mockPromoService = new Mock<PromotionServices>();

			var mockDisposableContext = mockPromoService.As<IDisposable>();

			mockDisposableContext.Setup(mdc => mdc.Dispose());

			var testController = new PromotionController(mockPromoService.Object);

			testController.Dispose();

			mockDisposableContext.Verify(mdc => mdc.Dispose(), Times.Never);
		}

		/// <summary>
		/// Verifies that the <see cref="PromotionController"/> when NOT given a
		/// <see cref="PromotionServices"/> instance at construction DOES call
		/// <see cref="IDisposable.Dispose"/> when the controller is disposed.
		/// </summary>
		/// <remarks>
		/// When a <see cref="PromotionServices"/> instance is not provided to a controller, the
		/// controller takes responsibility to create one when needed. When the PromoContext is
		/// created internally to the controller, then the controller is responsible for the
		/// lifetime of the service. Thus it is responsible to Dispose of the service when the
		/// controller is Dispose(d).
		/// </remarks>
		[TestMethod, TestCategory(Constants.TEST_CATEGORY_CHECK_IN)]
		public void Test_ConstructWithoutPromoContext_DisposesPromoContext()
		{
			var testController = new PromotionController();

			var promotionService1 = testController.PromoContext;

			testController.Dispose();

			var promotionService2 = testController.PromoContext;

			Assert.AreNotSame(promotionService1, promotionService2,
				"After Dispose() a new PromoContext should be returned.");
		}

		/// <summary>
		/// Verifies that the <see cref="BaseApiController"/> when given a
		/// <see cref="RolloutManagementContext"/> at construction does not call
		/// <see cref="IDisposable.Dispose"/> when the controller is disposed.
		/// </summary>
		/// <remarks>
		/// It is assumed that when the <see cref="RolloutManagementContext"/> is provided to the
		/// controller's constructor that the consumer of the controller, which is also the provider
		/// of the <see cref="BaseApiController.RmsDbContext"/>, is responsible for the lifetime of
		/// the database context and thus for disposing it. So the controller should not call
		/// Dispose on the context.
		/// </remarks>
		[TestMethod, TestCategory(Constants.TEST_CATEGORY_CHECK_IN)]
		public new void Test_ConstructWithRmsDbContext_DoesNotDisposeRmsDbContext()
		{
			var mockRmsDbContext = new Mock<RolloutManagementContext>();

			var mockDisposableContext = mockRmsDbContext.As<IDisposable>();

			mockDisposableContext.Setup(mdc => mdc.Dispose());

			var testController = new PromotionController(mockRmsDbContext.Object);

			testController.Dispose();

			mockDisposableContext.Verify(mdc => mdc.Dispose(), Times.Never);
		}

		/// <summary>
		/// Verifies that the <see cref="PromotionController"/> when NOT given a
		/// <see cref="RolloutManagementContext"/> at construction DOES call
		/// <see cref="IDisposable.Dispose"/> when the controller is disposed.
		/// </summary>
		/// <remarks>
		/// When a <see cref="RolloutManagementContext"/> is not provided to a controller, the
		/// controller takes responsibility to create one when needed. When the RmsDbContext is
		/// created internally to the controller, then the controller is responsible for the
		/// lifetime of the database context. Thus it is responsible to Dispose of the context
		/// when the controller is Dispose(d).
		/// </remarks>
		[TestMethod, TestCategory(Constants.TEST_CATEGORY_CHECK_IN)]
		public new void Test_ConstructWithoutRmsDbContext_DisposesRmsDbContext()
		{
			var testController = new PromotionController();

			var rmsDbContext1 = testController.RmsDbContext;

			testController.Dispose();

			var rmsDbContext2 = testController.RmsDbContext;

			Assert.AreNotSame(rmsDbContext1, rmsDbContext2,
				"After Dispose() a new RmsDbContext should be returned.");
		}

		#endregion Dispose(bool) Test Methods

	}
}
