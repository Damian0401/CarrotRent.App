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
import { CLIENT, EMPLOYEE, MANAGER } from "../common/utils/constants";
import VehicleCreate from "../../features/vehicle/create/VehicleCreate";
import { ToastContainer } from "react-toastify";
import AccessDenied from "../../features/error/AccessDenied";
import { UserUnverified } from "../../features/user/unverified/UserUnverified";
import RentArchive from "../../features/rent/archive/RentArchive";
import CreateEmployee from "../../features/account/CreateEmployee";

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
          <Route path='users/unverified' element={<PrivateRoute
            roles={[EMPLOYEE, MANAGER]}
            Component={UserUnverified}
          />} />
          <Route path='map' element={<Map />} />
          <Route path='departments'>
            <Route index element={<DepartmentDashboard />} />
            <Route path=':departmentId'>
              <Route index element={<DepartmentDetails />} />
              <Route path='employee'
                element={<PrivateRoute
                  roles={[MANAGER]}
                  Component={CreateEmployee}
                />}
              />
              <Route path='rents'>
                <Route index
                  element={<PrivateRoute
                    Component={RentDashboard}
                  />}
                />
                <Route path='archive'
                  element={<PrivateRoute
                    roles={[MANAGER, EMPLOYEE]}
                    Component={RentArchive}
                  />}
                />
              </Route>
            </Route>
          </Route>
          <Route path='vehicles'>
            <Route index element={<VehicleDashboard />} />
            <Route path='create/:departmentId'
              element={<PrivateRoute
                roles={[MANAGER]}
                Component={VehicleCreate}
              />}
            />
            <Route path='edit/:id'
              element={<PrivateRoute
                roles={[MANAGER, EMPLOYEE]}
                Component={VehicleEdit}
              />}
            />
            <Route path=':id' element={<VehicleDetails />} />
          </Route>
          <Route path='rents'>
            <Route index
              element={<PrivateRoute
                roles={[CLIENT]}
                Component={RentDashboard}
              />}
            />
            <Route path='archive'
              element={<PrivateRoute
                roles={[CLIENT]}
                Component={RentArchive}
              />}
            />
            <Route path=':rentId'
              element={<PrivateRoute
                roles={[CLIENT, EMPLOYEE, MANAGER]}
                Component={RentDetails}
              />}
            />
          </Route>
          <Route path='accessDenied' element={<AccessDenied />} />
          <Route path='*' element={<NotFound />} />
        </Route>
      </Routes>
    </div>
  );
}

export default App;
