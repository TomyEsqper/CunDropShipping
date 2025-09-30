using System.Collections.Generic;

namespace CunDropShipping.domain.Entity;
// Representa la idea pura de una orden de compra dentro del dominio.
public class PurchaseCommand
{
    public List<PurchaseItem> Items { get; set; }
}

public class PurchaseItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}