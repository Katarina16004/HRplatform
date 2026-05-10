import type { Skill } from "./Skill";
export interface Candidate {
    id: string;
    fullName: string;
    dateOfBirth: string; 
    email: string;
    contactNum: string;
    skills?: Skill[]; 
}
export interface CreateUpdateCandidateRequest {
    fullName: string;
    dateOfBirth: string | null;
    email: string;
    contactNum: string;
}