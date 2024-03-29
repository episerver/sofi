﻿using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using Foundation.Features.Checkout.Services;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Infrastructure.Commerce.Customer.Services;
//using Foundation.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.MyAccount.OrderConfirmation
{
    public class OrderConfirmationController : OrderConfirmationControllerBase<OrderConfirmationPage>
    {
        //private readonly ICampaignService _campaignService;
        private readonly IContextModeResolver _contextModeResolver;
        public OrderConfirmationController(
            //ICampaignService campaignService,
            IConfirmationService confirmationService,
            IAddressBookService addressBookService,
            IOrderGroupCalculator orderGroupCalculator,
            UrlResolver urlResolver, ICustomerService customerService,
            IContextModeResolver contextModeResolver) :
            base(confirmationService, addressBookService, orderGroupCalculator, urlResolver, customerService)
        {
            //_campaignService = campaignService;
            _contextModeResolver = contextModeResolver;
        }
        public ActionResult Index(OrderConfirmationPage currentPage, string notificationMessage, int? orderNumber)
        {
            IPurchaseOrder order = null;
            if (_contextModeResolver.CurrentMode.EditOrPreview())
            {
                order = _confirmationService.CreateFakePurchaseOrder();
            }
            else if (orderNumber.HasValue)
            {
                order = _confirmationService.GetOrder(orderNumber.Value);
            }

            if (order != null && order.CustomerId == _customerService.CurrentContactId)
            {
                var viewModel = CreateViewModel(currentPage, order);
                viewModel.NotificationMessage = notificationMessage;

                //_campaignService.UpdateLastOrderDate();
                //_campaignService.UpdatePoint(decimal.ToInt16(viewModel.SubTotal.Amount));

                return View(viewModel);
            }

            return Redirect(Url.ContentUrl(ContentReference.StartPage));
        }
    }
}