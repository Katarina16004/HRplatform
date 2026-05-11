import type { Skill } from "../../models/Skill";
import SkillRow from './SkillRow';

interface Props {
    skills: Skill[];
    onDelete: (id: string) => void;
}

const SkillTable = ({ skills, onDelete }: Props) => {
    return (
        <table cellPadding={10} style={{ borderCollapse: 'collapse', width: '100%', textAlign: 'left', borderRadius: '4px', overflow: 'hidden' }}>
            <thead style={{ backgroundColor: '#c3e1ff' }}>
                <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th style={{ width: '100px', textAlign: 'center' }}>Delete</th>
                </tr>
            </thead>
            <tbody>
                {skills.length > 0 ? (
                    skills.map(skill => (
                        <SkillRow 
                            key={skill.id} 
                            skill={skill} 
                            onDelete={onDelete} 
                        />
                    ))
                ) : (
                    <tr>
                        <td colSpan={3} style={{ textAlign: 'center', padding: '20px' }}>
                            No skills found.
                        </td>
                    </tr>
                )}
            </tbody>
        </table>
    );
};

export default SkillTable;