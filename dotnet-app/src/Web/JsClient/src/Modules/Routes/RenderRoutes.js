import flattenDeep from 'lodash/flattenDeep';
import React from 'react';
import { Route, Routes as ReactRoutes } from 'react-router-dom';
import ProtectedRoute from './ProtectedRoute';

function generateFlattenRoutes(routes) {
    if (!routes) return [];
    return flattenDeep(routes.map(({ routes: subRoutes, ...rest}) => [rest, generateFlattenRoutes(subRoutes)]));
}

export function RenderRoutes(mainRoutes) {
    const Routes = ({ isAuthorized }) => {
      const layouts = mainRoutes.map(({ layout: Layout, routes }, index) => {
        const subRoutes = generateFlattenRoutes(routes);
  
        return (
            <Route key={index} element={Layout}>
                {subRoutes.map(({ component: Component, path, name, isPublic }) => {
                return (
                    <Route key={name} element={<ProtectedRoute isPublic={isPublic} isAuthorized={isAuthorized} />}>
                        Component && path && (<Route key={name} element={Component} path={path} />)
                    </Route>
                )
                })}
            </Route>
        )
      });
      return <ReactRoutes>{layouts}</ReactRoutes>;
    }
    return Routes;
  }