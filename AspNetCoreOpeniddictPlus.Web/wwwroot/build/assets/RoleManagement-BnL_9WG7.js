import{j as e,L as g}from"./App-C_R1tlV6.js";import{B as c,D as u,a as p,b as f,c as v,d as w,L as d,I as N,T as y,e as b,u as T,A,C,f as P,g as D,h as L,i as M,j as x,k as l,l as R,m as r,P as z,n as H,o as h,p as _,q as k,r as I}from"./useError-DwYUvx16.js";function V({role:s}){return e.jsxs("form",{method:"post",action:"/roles/delete",children:[e.jsx("span",{className:"hidden",dangerouslySetInnerHTML:{__html:window.__RequestVerificationToken}}),e.jsx("input",{type:"hidden",name:"id",value:s.id}),e.jsx(c,{variant:"destructive",size:"icon",type:"submit",children:e.jsx("svg",{xmlns:"http://www.w3.org/2000/svg",width:"16",height:"16",fill:"currentColor",className:"bi bi-archive-fill",viewBox:"0 0 16 16",children:e.jsx("path",{d:"M12.643 15C13.979 15 15 13.845 15 12.5V5H1v7.5C1 13.845 2.021 15 3.357 15zM5.5 7h5a.5.5 0 0 1 0 1h-5a.5.5 0 0 1 0-1M.8 1a.8.8 0 0 0-.8.8V3a.8.8 0 0 0 .8.8h14.4A.8.8 0 0 0 16 3V1.8a.8.8 0 0 0-.8-.8z"})})})]})}function S({role:s}){return e.jsxs(u,{children:[e.jsx(p,{asChild:!0,children:e.jsx(c,{size:"icon",variant:"secondary",children:e.jsxs("svg",{xmlns:"http://www.w3.org/2000/svg",width:"16",height:"16",fill:"currentColor",className:"bi bi-pencil-square",viewBox:"0 0 16 16",children:[e.jsx("path",{d:"M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"}),e.jsx("path",{"fill-rule":"evenodd",d:"M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"})]})})}),e.jsxs(f,{className:"sm:oidc-max-w-screen-md",children:[e.jsx(v,{children:e.jsxs(w,{className:"oidc-capitalize",children:["Edit ",s.name]})}),e.jsxs("form",{method:"post",action:"/roles/update",children:[e.jsx("span",{className:"hidden",dangerouslySetInnerHTML:{__html:window.__RequestVerificationToken}}),e.jsx("input",{type:"hidden",name:"id",value:s.id}),e.jsxs("div",{className:"grid oidc-grid-cols-8 gap-4 py-4",children:[e.jsxs("div",{className:"flex oidc-flex-col oidc-col-span-8 oidc-space-y-1",children:[e.jsx(d,{htmlFor:"name",children:"Role Name"}),e.jsx(N,{id:"name",name:"name",defaultValue:s.name,className:"oidc-w-full"})]}),e.jsxs("div",{className:"flex oidc-flex-col oidc-col-span-8 oidc-space-y-1",children:[e.jsx(d,{htmlFor:"description",children:"Description"}),e.jsx(y,{id:"description",name:"description",defaultValue:s.description,className:"oidc-w-full"})]})]}),e.jsx(b,{children:e.jsx(c,{type:"submit",children:"Submit"})})]})]})]})}function B(s){const{roles:n,pageInfo:i,error:t}=s;console.log(n);const o=T({error:t,autoHide:!0});return e.jsxs("div",{className:"overflow-x-auto",children:[o&&e.jsxs(A,{variant:"destructive",children:[e.jsx(C,{className:"h-4 w-4"}),e.jsx(P,{children:"Error"}),e.jsx(D,{children:t})]}),e.jsxs(L,{className:"mt-5",children:[e.jsx(M,{children:e.jsxs(x,{className:"oidc-text-lg",children:[e.jsx(l,{children:"Name"}),e.jsx(l,{children:"Description"}),e.jsx(l,{children:"Assigned permissions"}),e.jsx(l,{children:"Action"})]})}),e.jsx(R,{children:n.map(a=>e.jsxs(x,{children:[e.jsx(r,{children:a.name}),e.jsx(r,{children:a.description??"N/A"}),e.jsxs(r,{children:["Total permissions (",a.rolePermissions.length,")"]}),e.jsxs(r,{className:"flex  oidc-space-x-2",children:[e.jsx(S,{role:a}),e.jsx(V,{role:a})]})]},a.id))})]}),i.totalCount>10&&e.jsx("div",{className:"flex overflow-x-auto justify-center pt-5",children:e.jsx(z,{children:e.jsxs(H,{children:[e.jsx(h,{children:e.jsx(_,{href:"#"})}),e.jsxs("span",{className:"text-sm text-gray-700 dark:text-gray-400",children:["Showing ",e.jsx("span",{className:"font-semibold text-gray-900 dark:text-white",children:i.currentPage})," to ",e.jsx("span",{className:"font-semibold text-gray-900 dark:text-white",children:i.pageSize})," of ",e.jsx("span",{className:"font-semibold text-gray-900 dark:text-white",children:i.totalCount})," Entries"]}),e.jsx(h,{children:e.jsx(k,{href:"#"})})]})})})]})}function F(s){console.log(s);const{items:n,currentPage:i,pageSize:t,hasPreviousPage:o,hasNextPage:a,totalCount:m}=s.data,j={currentPage:i,pageSize:t,totalCount:m,hasNextPage:a,hasPreviousPage:o};return e.jsxs(e.Fragment,{children:[e.jsx(g,{children:e.jsx("title",{children:"Admin | Role Management"})}),e.jsx(I,{title:"Role Management",createType:"role",children:e.jsx(B,{roles:n,pageInfo:j,error:s.error})})]})}export{F as default};
