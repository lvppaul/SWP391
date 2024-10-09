import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import App from "./App";
import reportWebVitals from "./reportWebVitals";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.css";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Home from "./pages/Home/Home";
import Login from "./pages/Login/Login";
import Signup from "./pages/Signup/Signup";
import Shop from "./pages/Shop/Shop";
import NotPage from "./pages/NotPage/NotPage";
import Pond from "./pages/Pond/Pond";
import AdminHome from "./pages/Admin/Home/AdminHome";
import TableUser from "./components/TableUser/TableUser";
import SaltCalculator from "./pages/SaltCalculator/SaltCalculator";
import FoodCalculator from "./pages/FoodCalculator/FoodCalculator";
import Product from "./pages/Product/Product";
import PondDetail from "./pages/PondDetail/PondDetail";
import KoiDetail from "./pages/KoiDetails/KoiDetail";
// import AddNewBlog from "./components/AddNewBlog/AddNewBlog";
import Blog from "./pages/Blog/Blog";
import AuthProvider from "./pages/Login/AuthProvider";
import ProtectedRoute from "./components/ProtectedRoute"; // Import the ProtectedRoute component

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path="/" element={<App />}>
            <Route index element={<Home />} />
            <Route path="shop" element={<Shop />} />
            <Route path="product/:productId" element={<Product />} />
            <Route path="pond" element={<Pond />} />
            <Route path="foodcalculator" element={<FoodCalculator />} />
            <Route path="saltcalculator" element={<SaltCalculator />} />
            <Route path="blogs" element={<Blog />} />
            <Route path="news" element={<h1>News</h1>} />
            <Route path="*" element={<NotPage />} />
            <Route path="ponddetail" element={<PondDetail />} />
            <Route path="koidetail" element={<KoiDetail />} />
            <Route />
          </Route>

          <Route path="login" element={<Login />} />
          <Route path="signup" element={<Signup />} />
          <Route
            path="/admin/"
            element={
              <ProtectedRoute requiredRole="admin">
                <AdminHome />
              </ProtectedRoute>
            }
          >
            <Route index element={<h1>Report</h1>} />
            <Route path="report" element={<h1>Report</h1>} />
            <Route
              path="usermanage"
              element={
                <ProtectedRoute requiredRole="admin">
                  <TableUser />
                </ProtectedRoute>
              }
            />
            <Route
              path="shopadmin"
              element={
                <ProtectedRoute requiredRole="admin">
                  <h1>Shop</h1>
                </ProtectedRoute>
              }
            />
            <Route
              path="products"
              element={
                <ProtectedRoute requiredRole="admin">
                  <h1>Products</h1>
                </ProtectedRoute>
              }
            />
            <Route
              path="catagories"
              element={
                <ProtectedRoute requiredRole="admin">
                  <h1>Categories</h1>
                </ProtectedRoute>
              }
            />
            <Route
              path="setting"
              element={
                <ProtectedRoute requiredRole="admin">
                  <h1>Setting</h1>
                </ProtectedRoute>
              }
            />
            <Route
              path="feedback"
              element={
                <ProtectedRoute requiredRole="admin">
                  <h1>Feedback</h1>
                </ProtectedRoute>
              }
            />
          </Route>
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  </React.StrictMode>
);

// Performance measurement
reportWebVitals();
