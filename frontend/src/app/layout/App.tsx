import { Route, Routes } from "react-router-dom";
import LoginForm from "../../features/account/LoginForm";
import VehicleDetails from "../../features/vehicle/details/VehicleDetails";
import DepartmentDashboard from "../../features/department/dashboard/DepartmentDashboard";
import DepartmentDetails from "../../features/department/details/DepartmentDetails";
import NotFound from "../../features/error/NotFound";
import HomePage from "../../features/home/HomePage";
import Map from "../../features/map/Map";
import Navbar from "./Navbar";
import ToggleThemeButton from "./ToggleThemeButton";
import RegisterForm from "../../features/account/RegisterForm";
import VehicleDashboard from "../../features/vehicle/dashboard/VehicleDashboard";
import RentDashboard from "../../features/rent/dashboard/RentDashboard";
import RentDetails from "../../features/rent/details/RentDetails";
import PrivateRoute from "../common/shared/PrivateRoute";
import AnonymousRoute from "../common/shared/AnonymousRoute";
import VehicleEdit from "../../features/vehicle/edit/VehicleEdit";
import { EMPLOYEE, MANAGER } from "../common/utils/constants";
import VehicleCreate from "../../features/vehicle/create/VehicleCreate";
import { ToastContainer } from "react-toastify";
import AccessDenied from "../../features/error/AccessDenied";

function App() {
  return (
    <div style={{ height: '93vh', width: '100vw' }}>
      <ToastContainer position='bottom-right' hideProgressBar />
      <ToggleThemeButton />
      <Routes>
        <Route element={<Navbar />}>
          <Route index element={<HomePage />} />
          <Route path='login' element={<AnonymousRoute Component={LoginForm} />} />
          <Route path='register' element={<AnonymousRoute Component={RegisterForm} />} />
          <Route path='map' element={<Map />} />
          <Route path='departments'>
            <Route index element={<DepartmentDashboard />} />
            <Route path=':id' element={<DepartmentDetails />} />
          </Route>
          <Route path='vehicles'>
            <Route index element={<VehicleDashboard />} />
            <Route
              path='create/:departmentId'
              element={<PrivateRoute
                roles={[MANAGER]}
                Component={VehicleCreate}
              />}
            />
            <Route
              path='edit/:id'
              element={<PrivateRoute
                roles={[MANAGER, EMPLOYEE]}
                Component={VehicleEdit}
              />}
            />
            <Route path=':id' element={<VehicleDetails />} />
          </Route>
          <Route path='rents'>
            <Route index element={<RentDashboard />} />
            <Route path=':id' element={<RentDetails />} />
          </Route>
          <Route path='access-denied' element={<AccessDenied />} />
          <Route path='*' element={<NotFound />} />
        </Route>
      </Routes>
    </div>
  );
}

export default App;
