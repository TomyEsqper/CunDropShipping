using System.Collections.Generic;
namespace CunDropShipping.adapter.restful.v1.controller.Entity;

public class PurchaseRequest
{
    public List<PurchaseItemRequest> Items { get; set; }
}