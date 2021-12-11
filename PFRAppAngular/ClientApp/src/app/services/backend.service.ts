import {HttpClient} from "@angular/common/http";
import {Inject, Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {EmployeeModel} from "../models/employeeModel";
import {DocumentModel} from "../models/documentModel";
import {OrganizationModel} from "../models/organizationModel";


@Injectable()
export class BackendService {
  private readonly baseUrl: string;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = `${baseUrl}api`;
  }

  public choiceMethod(method: string, searchTerm: string): Observable<any> {
    switch (method) {
      case 'organization':{
        return this.getOrganizations(searchTerm);
      }
      case 'employees': {
        return this.getEmployees(searchTerm)
      }
      case 'documents': {
        return this.getDocuments(searchTerm)
      }
    }
  }

  public getDocuments(searchTerm: string = null): Observable<any> {
    return this.http
      .get<DocumentModel[]>(this.baseUrl + `/document/list?searchTerm=${searchTerm}`)
      .pipe(map(resp => {
          return resp;
        })
      );
  }

  public addDocument(document: DocumentModel): Observable<any> {
    return this.http.post<DocumentModel>(this.baseUrl + '/document/add', document);
  }

  public generateFile(id: number, keyValuePairs: string): Observable<any> {
    let url = this.baseUrl + `/document/generate/${id}?placeholdersValues=${keyValuePairs}`
    return this.http.get(url, {
      responseType: 'blob',
      observe: 'response'
    })
      .pipe(
        map((res: any) => {
          return new Blob([res.body], {type: 'docx'});
        })
      );
  }

  public getEmployees(organizationId: string, searchTerm: string = null): Observable<any> {
    return this.http
      .get<EmployeeModel[]>(this.baseUrl + `/employee/list/${organizationId}?searchTerm=${searchTerm}`)
      .pipe(map(resp => {
          return resp;
        })
      );
  }

  private getOrganizations(searchTerm: string = null): Observable<any> {
    return this.http
      .get<OrganizationModel[]>(this.baseUrl + `/organization/get-all?searchTerm=${searchTerm}`)
      .pipe(map(resp => {
          return resp;
        })
      );
  }
}
