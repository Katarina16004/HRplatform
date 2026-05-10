import type { CreateSkillRequest, Skill } from "../../models/Skill";

export interface ISkillAPIService {
    getAllSkills(): Promise<Skill[]>;
    getSkillById(id: string): Promise<Skill | null>;
    getSkillByName(name: string): Promise<Skill | null>;
    
    createSkill(skill: CreateSkillRequest): Promise<string>;
    deleteSkill(id: string): Promise<boolean>;
}