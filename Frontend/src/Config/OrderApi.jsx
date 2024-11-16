import api from "./AxiosConfig";

// Function to create an order
const createOrder = async (orderData) => {
  try {
    const response = await api.post("Order", orderData, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error creating order:", error);
    return error.response;
  }
};

const getOrderById = async (orderId) => {
  try {
    const response = await api.get(`Order/OrderId/${orderId}`, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching order:", error);
    return null;
  }
};

const getOrderByUserId = async (userId) => {
  try {
    const response = await api.get(`Order/UserId/${userId}`, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching order:", error);
    return null;
  }
};

const getOrderByShopId = async (shopId) => {
  try {
    const response = await api.get(`Order/ShopId/${shopId}`, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching order:", error);
    return null;
  }
};

const getVipOrderByUserId = async (userId) => {
  try {
    const response = await api.get(`Order/VipOrderByUserId/${userId}`, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching order:", error);
    return null;
  }
};
/* ------------------------Commission Order---------------------  */
// get list commission fee order
const getListOrder = async () => {
  try {
    const response = await api.get(`Order`, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching order:", error);
    return null;
  }
};

// get Commission order by days - https://localhost:7062/api/Order/orders-by-day?days=30
const getListOrderByDays = async (day) => {
  try {
    const response = await api.get(`Order/orders-by-day?days=${day}`, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching order:", error);
    return null;
  }
};

const getListShopOrderByDays = async (shopId, day) => {
  try {
    const response = await api.get(`Order/orders-for-shop-by-day?shopId=${shopId}&days=${day}`, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching order:", error);
    return null;
  }
}

/*------------------- Vip Order --------------- */
// get list Vip package order
const getListVipOrder = async () => {
  try {
    const response = await api.get(`Order/GetVipOrder`, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching order:", error);
    return null;
  }
};
// get list Vip Order package Order By Week - https://localhost:7062/api/Order/vip-orders-by-week?weeks=1

const getListVipOrderByWeek = async (week) => {
  try {
    const response = await api.get(`Order/vip-orders-by-week?weeks=${week}`, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching order:", error);
    return null;
  }
};

// get vip orders by month https://localhost:7062/api/Order/vip-orders-by-month?month=3
const getListVipOrderByMonth = async (month) => {
  try {
    const response = await api.get(`Order/vip-orders-by-month?month=${month}`, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching order:", error);
    return null;
  }
};
// get vip orders by days - https://localhost:7062/api/Order/vip-orders-by-day?days=7
const getListVipOrderByDays = async (days) => {
  try {
    const response = await api.get(`Order/vip-orders-by-day?days=${days}`, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching order:", error);
    return null;
  }
};

const setOrderSuccess = async (orderId) => {
  try {
    const response = await api.post(`Order/${orderId}/set-successful`, {
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    console.error("Error setting order success:", error);
    return null;
  }
}

export {
  createOrder,
  getOrderById,
  getOrderByUserId,
  getOrderByShopId,
  getVipOrderByUserId,
  getListOrder,
  getListVipOrder,
  getListVipOrderByWeek,
  getListVipOrderByMonth,
  getListVipOrderByDays,
  getListOrderByDays,
  getListShopOrderByDays,
  setOrderSuccess,
};
