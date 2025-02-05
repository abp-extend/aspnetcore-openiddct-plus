import{j as e,L as g}from"./App-C_R1tlV6.js";import{B as x,D as f,a as p,b as N,c as v,d as w,L as o,I as c,e as b,u as y,A,C,f as T,g as L,h as P,i as q,j as h,k as l,l as z,s as D,m as t,P as M,n as H,o as m,p as I,q as R,r as U}from"./useError-DwYUvx16.js";function V({user:s}){return e.jsxs("form",{method:"post",action:"/users/delete",children:[e.jsx("span",{className:"hidden",dangerouslySetInnerHTML:{__html:window.__RequestVerificationToken}}),e.jsx("input",{type:"hidden",name:"id",value:s.id}),e.jsx(x,{variant:"destructive",size:"icon",type:"submit",disabled:!!s.deletionRequestedAt,children:e.jsx("svg",{xmlns:"http://www.w3.org/2000/svg",width:"16",height:"16",fill:"currentColor",className:"bi bi-archive-fill",viewBox:"0 0 16 16",children:e.jsx("path",{d:"M12.643 15C13.979 15 15 13.845 15 12.5V5H1v7.5C1 13.845 2.021 15 3.357 15zM5.5 7h5a.5.5 0 0 1 0 1h-5a.5.5 0 0 1 0-1M.8 1a.8.8 0 0 0-.8.8V3a.8.8 0 0 0 .8.8h14.4A.8.8 0 0 0 16 3V1.8a.8.8 0 0 0-.8-.8z"})})})]})}function _({user:s}){return e.jsxs(f,{children:[e.jsx(p,{asChild:!0,children:e.jsx(x,{size:"icon",variant:"secondary",disabled:!!s.deletionRequestedAt,children:e.jsxs("svg",{xmlns:"http://www.w3.org/2000/svg",width:"16",height:"16",fill:"currentColor",className:"bi bi-pencil-square",viewBox:"0 0 16 16",children:[e.jsx("path",{d:"M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"}),e.jsx("path",{"fill-rule":"evenodd",d:"M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"})]})})}),e.jsxs(N,{className:"sm:oidc-max-w-screen-md",children:[e.jsx(v,{children:e.jsxs(w,{className:"oidc-capitalize",children:["Edit ",s.userName]})}),e.jsxs("form",{method:"post",action:"/users/update",children:[e.jsx("span",{className:"hidden",dangerouslySetInnerHTML:{__html:window.__RequestVerificationToken}}),e.jsx("input",{type:"hidden",name:"id",value:s.id}),e.jsxs("div",{className:"grid oidc-grid-cols-8 gap-4 py-4",children:[e.jsxs("div",{className:"flex oidc-flex-col oidc-col-span-4 oidc-space-y-1",children:[e.jsx(o,{htmlFor:"firstName",children:"First Name"}),e.jsx(c,{id:"firstName",name:"firstName",defaultValue:s.firstName,className:"oidc-w-full"})]}),e.jsxs("div",{className:"flex oidc-flex-col oidc-col-span-4 oidc-space-y-1",children:[e.jsx(o,{htmlFor:"lastName",children:"Last Name"}),e.jsx(c,{id:"lastName",name:"lastName",defaultValue:s.lastName,className:"oidc-w-full"})]}),e.jsxs("div",{className:"flex oidc-flex-col oidc-col-span-8 oidc-space-y-1",children:[e.jsx(o,{htmlFor:"username",children:"Username"}),e.jsx(c,{id:"username",name:"username",defaultValue:s.userName,required:!0})]})]}),e.jsx(b,{children:e.jsx(x,{type:"submit",children:"Submit"})})]})]})]})}function k(s){const{users:n,pageInfo:i,error:r}=s;console.log(n);const d=y({error:r,autoHide:!0});return e.jsxs("div",{className:"overflow-x-auto",children:[d&&e.jsxs(A,{variant:"destructive",children:[e.jsx(C,{className:"h-4 w-4"}),e.jsx(T,{children:"Error"}),e.jsx(L,{children:r})]}),e.jsxs(P,{className:"mt-5",children:[e.jsx(q,{children:e.jsxs(h,{className:"oidc-text-lg",children:[e.jsx(l,{children:" First name"}),e.jsx(l,{children:"Username"}),e.jsx(l,{children:"Email"}),e.jsx(l,{children:"Email verified"}),e.jsx(l,{children:"Action"})]})}),e.jsx(z,{children:n.map(a=>e.jsxs(h,{className:D({"oidc-bg-amber-100 hover:oidc-bg-amber-100 hover:oidc-cursor-not-allowed":a.deletionRequestedAt}),title:a.deletionRequestedAt?"Deletion requested and it will be deleted in 30 days.":"",children:[e.jsx(t,{children:a.firstName??"N/A"}),e.jsx(t,{children:a.userName}),e.jsx(t,{children:a.email}),e.jsx(t,{children:a.emailConfirmed?"Yes":"No"}),e.jsxs(t,{className:"flex  oidc-space-x-2",children:[e.jsx(_,{user:a}),e.jsx(V,{user:a})]})]},a.id))})]}),i.totalCount>10&&e.jsx("div",{className:"flex overflow-x-auto justify-center pt-5",children:e.jsx(M,{children:e.jsxs(H,{children:[e.jsx(m,{children:e.jsx(I,{href:"#"})}),e.jsxs("span",{className:"text-sm text-gray-700 dark:text-gray-400",children:["Showing ",e.jsx("span",{className:"font-semibold text-gray-900 dark:text-white",children:i.currentPage})," to ",e.jsx("span",{className:"font-semibold text-gray-900 dark:text-white",children:i.pageSize})," of ",e.jsx("span",{className:"font-semibold text-gray-900 dark:text-white",children:i.totalCount})," Entries"]}),e.jsx(m,{children:e.jsx(R,{href:"#"})})]})})})]})}function S(s){const{items:n,currentPage:i,pageSize:r,hasPreviousPage:d,hasNextPage:a,totalCount:j}=s.data,u={currentPage:i,pageSize:r,totalCount:j,hasNextPage:a,hasPreviousPage:d};return e.jsxs(e.Fragment,{children:[e.jsx(g,{children:e.jsx("title",{children:"Admin | User Management"})}),e.jsx(U,{title:"User Management",createType:"user",children:e.jsx(k,{users:n,pageInfo:u,error:s.error})})]})}export{S as default};
