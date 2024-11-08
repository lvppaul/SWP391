import { useState, useEffect } from "react";
import { getListOrder } from "../../Config/OrderApi";
import Button from "@mui/material/Button";

import AdminViewOrderDetailDialog from "./AdminViewOrderDetail";
const AdminOrderManagement = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchOrders = async () => {
    try {
      const order = await getListOrder();
      setOrders(order);
    } catch (error) {
      console.log(error.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchOrders();
  }, []);

  if (loading) return <div>Loading...</div>;
  return (
    <div className="right-content">
      <div className="members-content shadow border-0 p-3 mt-4">
        <div className="member-content-header d-flex ">
          <h3 className="hd">Order Management</h3>
        </div>
        <div className="table-response">
          <table className="table table-sm">
            <thead>
              <tr>
                <th>Order ID</th>
                <th>User Name</th>
                <th>Email</th>
                <th>Phone</th>
                <th>Create At</th>
                <th>Total Price</th>
                <th>View</th>
              </tr>
            </thead>
            <tbody>
              {orders.map((order) => (
                <tr key={order.id}>
                  <td>{order.orderId}</td>
                  <td>{order.fullName}</td>
                  <td>{order.email}</td>
                  <td>{order.phone}</td>
                  <td>
                    {new Date(order.createDate).toLocaleDateString("vi-VN", {
                      year: "numeric",
                      month: "2-digit",
                      day: "2-digit",
                    })}
                  </td>
                  <td>
                    {" "}
                    {order.totalPrice.toLocaleString("vi-VN", {
                      style: "currency",
                      currency: "VND",
                    })}
                  </td>
                  <td>
                    <AdminViewOrderDetailDialog
                      orderDetail={order.orderDetails}
                      isVip={order.isVipUpgrade}
                    />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};
export default AdminOrderManagement;
