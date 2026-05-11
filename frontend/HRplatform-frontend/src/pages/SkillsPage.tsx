import { useEffect, useState } from 'react';
import { skillApi } from '../api_services/skill/SkillAPIService';
import type { Skill, CreateSkillRequest } from '../models/Skill';
import SkillForm from '../components/skill/CreateSkillForm';
import SkillTable from '../components/skill/SkillTable';
import SkillSearch from '../components/skill/SkillSearchForm';
import toast from 'react-hot-toast';

const SkillsPage = () => {
    const [skills, setSkills] = useState<Skill[]>([]);
    const [loading, setLoading] = useState(true);

    const refresh = async () => {
        setLoading(true);
        try {
            const data = await skillApi.getAllSkills();
            setSkills(data);
        } catch (err) {
            console.error("Error:", err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        refresh();
    }, []);

    const handleCreate = async (request: CreateSkillRequest) => {
        try {
            await skillApi.createSkill(request);
            toast.success("Skill created successfully!");
            await refresh();
        } catch (err:any) {
            console.error("Error:", err);
            toast.error(err.response?.data || "Error creating skill");
        }
    };

    const handleDelete = async (id: string) => {
        if (window.confirm("Are you sure you want to delete this skill?")) {
            try{
                await skillApi.deleteSkill(id);
                toast.success("Skill deleted successfully!");
                await refresh();
            }
            catch(err:any) {
                console.error("Error:", err);
                toast.error(err.response?.data || "Error deleting skill");
            }
        }
    };

    return (
        <div style={{ maxWidth: '90%', margin: '40px auto', padding: '20px', backgroundColor: 'white', borderRadius: '8px', boxShadow: '0 2px 10px #7998fb3d' }}>
            <h1 style={{ paddingBottom: '10px' }}>Skills</h1>
            
            <div style={{ 
                display: 'flex', 
                gap: '20px', 
                justifyContent: 'space-between', 
                alignItems: 'flex-start',
                marginBottom: '30px' 
                }}>

                <div style={{ flex: 1 }}>
                    <SkillForm onSkillCreated={handleCreate} />
                </div>

                <div style={{ flex: 1 }}>
                    <SkillSearch />
                </div>
            </div>

            <hr style={{ border: '0', borderTop: '1px solid #eee', marginBottom: '30px' }} />

            {loading && skills.length === 0 ? (
                <p>Loading data...</p>
            ) : (
                <SkillTable skills={skills} onDelete={handleDelete} />
            )}
        </div>
    );
};

export default SkillsPage;