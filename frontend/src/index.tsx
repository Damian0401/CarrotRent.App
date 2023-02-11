import { ChakraProvider } from '@chakra-ui/react';
import React from 'react';
import ReactDOM from 'react-dom/client';
import theme from './app/common/utils/theme';
import App from './app/layout/App';
import { unstable_HistoryRouter as HistoryRouter } from 'react-router-dom';
import { createBrowserHistory } from 'history';
import { UserProvider } from './app/common/providers/UserProvider';
import 'react-toastify/dist/ReactToastify.min.css';
import "react-datepicker/dist/react-datepicker.css";

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

export const history = createBrowserHistory({ window });

document.title = 'CarrotRent';

root.render(
  <React.StrictMode>
    <UserProvider>
    <ChakraProvider theme={theme}>
      <HistoryRouter history={history}>
        <App />
      </HistoryRouter>
    </ChakraProvider>
    </UserProvider>
  </React.StrictMode>
);